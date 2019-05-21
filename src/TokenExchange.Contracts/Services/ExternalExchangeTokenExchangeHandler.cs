using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using GraphQLPlay.Contracts;
using GraphQLPlay.IdentityModelExtras;
using IdentityModel.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using TokenExchange.Contracts.Models;
using Utils;
using Utils.Models;

namespace TokenExchange.Contracts.Services
{
    public static class ExternalExchangeTokenExchangeHandlerServiceCollectionExtensions
    {
        public static IServiceCollection AddPipelineTokenExchangeHandler(this IServiceCollection services, Func<IPipelineTokenExchangeHandler> valueFactory)
        {

            services.AddTransient((serviceProvider) =>
            {
                return new Lazy<IPipelineTokenExchangeHandler>(valueFactory);
            });

            return services;
        }
    }

    public class ExternalExchangeTokenExchangeHandler : IPipelineTokenExchangeHandler
    {
        public string Name => _externalExchangeRecord.ExchangeName;

        private IHttpContextAccessor _httpContextAssessor;
        private ITokenMintingService _tokenMintingService;
        private ISummaryLogger _summaryLogger;
        private ILogger<ExternalExchangeTokenExchangeHandler> _logger;
        private string _cacheKey;
        private IOptionsSnapshot<TokenClientOptions> _optionsSnapshot;
        private IMemoryCache _memoryCache;
        private ExternalExchangeRecord _externalExchangeRecord;
        private TokenClientOptions _settings;
        private string _name;
        private IDiscoveryCache _discoveryCache;
        private IDefaultHttpClientFactory _defaultHttpClientFactory;

        public ExternalExchangeTokenExchangeHandler(
            IDefaultHttpClientFactory defaultHttpClientFactory,
            IOptionsSnapshot<TokenClientOptions> optionsSnapshot,
            ITokenMintingService tokenMintingService,
            IMemoryCache memoryCache,
            IHttpContextAccessor httpContextAssessor,
            ISummaryLogger summaryLogger,
            ILogger<ExternalExchangeTokenExchangeHandler> logger
            )
        {
            _defaultHttpClientFactory = defaultHttpClientFactory;
            _cacheKey = "a9c4c7b7-dbb1-4d24-a78d-b8f89cc9ca83";
            _optionsSnapshot = optionsSnapshot;
            _memoryCache = memoryCache;
            _httpContextAssessor = httpContextAssessor;
            _tokenMintingService = tokenMintingService;
            _summaryLogger = summaryLogger;
            _logger = logger;
        }
        async Task<string> GetTokenAsync()
        {
            var httpClient = _defaultHttpClientFactory.HttpClient;
            TokenResponse tokenResponse = null;
            if (!_memoryCache.TryGetValue(_cacheKey, out tokenResponse))
            {
                // Key not in cache, so get data.
                var discoveryResponse = await DiscoveryCache.GetAsync();
                tokenResponse = await httpClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
                {
                    Address = discoveryResponse.TokenEndpoint,
                    ClientId = _settings.ClientId,
                    ClientSecret = _settings.ClientSecret
                });

                // Set cache options.
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    // Keep in cache for this time, reset time if accessed.
                    .SetSlidingExpiration(TimeSpan.FromMinutes(3));

                // Save data in cache.
                _memoryCache.Set(_cacheKey, tokenResponse, cacheEntryOptions);
                if (tokenResponse.IsError)
                {
                    _summaryLogger.Add("client_credentials_error", $"clientId:{_settings.ClientId} error:{tokenResponse.Error} endpoint:{discoveryResponse.TokenEndpoint}");
                }
            }
            return tokenResponse.AccessToken;
        }
        IDiscoveryCache DiscoveryCache
        {
            get
            {
                if (_discoveryCache == null)
                {
                    var httpClient = _defaultHttpClientFactory.HttpClient;
                    DiscoveryPolicy discoveryPolicy = new DiscoveryPolicy()
                    {
                        ValidateIssuerName = false,
                        ValidateEndpoints = false,
                    };

                    _discoveryCache = new DiscoveryCache(
                        _settings.Authority,
                        httpClient,
                        discoveryPolicy);
                }
                return _discoveryCache;
            }
        }
        public void Configure(ExternalExchangeRecord externalExchangeRecord)
        {
            _externalExchangeRecord = externalExchangeRecord;
            _settings = _optionsSnapshot.Get(_externalExchangeRecord.ExchangeName);
            _cacheKey = $"{_externalExchangeRecord.ExchangeName}_{_cacheKey}";


        }

        public class TokenExchangeRequestPackage : TokenExchangeRequest
        {
            public TokenExchangeRequestPackage() { }
            public TokenExchangeRequestPackage(TokenExchangeRequest ter)
            {
                Tokens = ter.Tokens;
                Extras = ter.Extras;
            }
            public Dictionary<string, List<KeyValuePair<string, string>>> MapOpaqueKeyValuePairs { get; set; }
        }
        public async Task<List<TokenExchangeResponse>> ProcessExchangeAsync(
            TokenExchangeRequest tokenExchangeRequest,
            Dictionary<string, List<KeyValuePair<string, string>>> mapOpaqueKeyValuePairs)
        {
            var access_token = await GetTokenAsync();
            if (string.IsNullOrEmpty(access_token))
            {
                throw new Exception("Unable to fetch client_credentials access_token");
            }

            var headers = new List<HttpHeader>(_externalExchangeRecord.oAuth2_client_credentials.AdditionalHeaders)
            {
                new HttpHeader() {Name = "Authorization", Value = $"Bearer {access_token}"},
                new HttpHeader() {Name = "Accept", Value = $"application/json"}

            };
            var passThrough = _externalExchangeRecord.MintType == "passThroughHandler";
            var externalUrl = passThrough ? _externalExchangeRecord.PassThroughHandler.Url : _externalExchangeRecord.ExternalFinalExchangeHandler.Url;

            (string content, HttpStatusCode statusCode) responseBag;
            using (var httpClient = _defaultHttpClientFactory.HttpClient)
            {
                responseBag = await Utils.EfficientApiCalls.HttpClientHelpers.PostStreamAsync(
                    _defaultHttpClientFactory.HttpClient,
                    externalUrl,
                    headers,
                    new TokenExchangeRequestPackage(tokenExchangeRequest)
                    {
                        MapOpaqueKeyValuePairs = mapOpaqueKeyValuePairs
                    },
                    CancellationToken.None);
            }

            if (responseBag.statusCode == HttpStatusCode.OK)
            {
                if (passThrough)
                {
                    var passThroughResult = JsonConvert.DeserializeObject<List<TokenExchangeResponse>>(responseBag.content);
                    return passThroughResult;
                }
                else
                {
                    var tokenExchangeResponses = new List<TokenExchangeResponse>();
                    var externalExchangeTokenRequests = JsonConvert.DeserializeObject<List<ExternalExchangeTokenResponse>>(responseBag.content);

                    foreach (var externalExchangeResourceOwnerTokenRequest in externalExchangeTokenRequests)
                    {
                        if (externalExchangeResourceOwnerTokenRequest.CustomTokenResponse != null)
                        {
                            var tokenExchangeResponse = new TokenExchangeResponse()
                            {
                                customToken = externalExchangeResourceOwnerTokenRequest.CustomTokenResponse
                            };
                            tokenExchangeResponses.Add(tokenExchangeResponse);
                        }
                        if (externalExchangeResourceOwnerTokenRequest.ArbitraryIdentityTokenRequest != null)
                        {
                            var arbitraryIdentityTokenRequest = externalExchangeResourceOwnerTokenRequest
                                .ArbitraryIdentityTokenRequest;
                            IdentityTokenRequest tokenRequest = new IdentityTokenRequest()
                            {
                                IdentityTokenLifetime = arbitraryIdentityTokenRequest.IdentityTokenLifetime,
                                ArbitraryClaims = arbitraryIdentityTokenRequest.ArbitraryClaims,
                                Scope = arbitraryIdentityTokenRequest.Scope,
                                Subject = arbitraryIdentityTokenRequest.Subject,
                                ClientId = _externalExchangeRecord.ExternalFinalExchangeHandler.ClientId // configured value
                            };

                            var response =
                                await _tokenMintingService.MintIdentityTokenAsync(tokenRequest);
                            if (response.IsError)
                            {
                                throw new Exception(response.Error);
                            }
                            var tokenExchangeResponse = new TokenExchangeResponse()
                            {
                                IdentityToken = new IdentityTokenResponse()
                                {
                                    hint = arbitraryIdentityTokenRequest.Hint,
                                    id_token = response.IdentityToken,
                                    expires_in = response.ExpiresIn,
                                    authority =
                                        $"{_httpContextAssessor.HttpContext.Request.Scheme}://{_httpContextAssessor.HttpContext.Request.Host}",
                                    HttpHeaders = arbitraryIdentityTokenRequest.HttpHeaders
                                }
                            };
                            tokenExchangeResponses.Add(tokenExchangeResponse);
                        }

                        if (externalExchangeResourceOwnerTokenRequest.ArbitraryResourceOwnerTokenRequest != null)
                        {

                            var arbitraryResourceOwnerTokenRequest = externalExchangeResourceOwnerTokenRequest
                                .ArbitraryResourceOwnerTokenRequest;
                            ResourceOwnerTokenRequest resourceOwnerTokenRequest = new ResourceOwnerTokenRequest()
                            {
                                AccessTokenLifetime = arbitraryResourceOwnerTokenRequest.AccessTokenLifetime,
                                ArbitraryClaims = arbitraryResourceOwnerTokenRequest.ArbitraryClaims,
                                Scope = arbitraryResourceOwnerTokenRequest.Scope,
                                Subject = arbitraryResourceOwnerTokenRequest.Subject,
                                ClientId = _externalExchangeRecord.ExternalFinalExchangeHandler.ClientId // configured value
                            };

                            var response =
                                await _tokenMintingService.MintResourceOwnerTokenAsync(resourceOwnerTokenRequest);
                            if (response.IsError)
                            {
                                throw new Exception(response.Error);
                            }

                            var tokenExchangeResponse = new TokenExchangeResponse()
                            {
                                accessToken = new AccessTokenResponse()
                                {
                                    hint = arbitraryResourceOwnerTokenRequest.Hint,
                                    access_token = response.AccessToken,
                                    refresh_token = response.RefreshToken,
                                    expires_in = response.ExpiresIn,
                                    token_type = response.TokenType,
                                    authority =
                                        $"{_httpContextAssessor.HttpContext.Request.Scheme}://{_httpContextAssessor.HttpContext.Request.Host}",
                                    HttpHeaders = arbitraryResourceOwnerTokenRequest.HttpHeaders
                                }
                            };
                            tokenExchangeResponses.Add(tokenExchangeResponse);
                        }

                    }
                    return tokenExchangeResponses;
                }
            }

            return null;
        }

        public static void RegisterServices(
            IServiceCollection services,
            IExternalExchangeStore tempExternalExchangeStore)
        {
            foreach (var exchange in tempExternalExchangeStore.GetExternalExchangeRecordAsync().GetAwaiter().GetResult())
            {
                services.Configure<TokenClientOptions>(exchange.ExchangeName, options =>
                 {
                     options.Authority = exchange.oAuth2_client_credentials.Authority;
                     options.ClientId = exchange.oAuth2_client_credentials.ClientId;
                     options.ClientSecret = exchange.oAuth2_client_credentials.ClientSecret;
                 });
                services.AddTransient<Lazy<IPipelineTokenExchangeHandler>>(serviceProvider =>
                {
                    return new Lazy<IPipelineTokenExchangeHandler>(() =>
                    {
                        var tokenExchangeHandler = serviceProvider.GetRequiredService<ExternalExchangeTokenExchangeHandler>();
                        tokenExchangeHandler.Configure(exchange);
                        return tokenExchangeHandler;
                    });
                });

            }
        }
    }
}