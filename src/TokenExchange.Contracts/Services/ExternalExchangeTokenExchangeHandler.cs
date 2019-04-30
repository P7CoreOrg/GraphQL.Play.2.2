using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using GraphQLPlay.Contracts;
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

        public ExternalExchangeTokenExchangeHandler(
            IOptionsSnapshot<TokenClientOptions> optionsSnapshot,
            ITokenMintingService tokenMintingService,
            IMemoryCache memoryCache,
            IHttpContextAccessor httpContextAssessor,
            ISummaryLogger summaryLogger,
            ILogger<ExternalExchangeTokenExchangeHandler> logger
            )
        {
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
            var httpClient = new HttpClient();
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
                    var httpClient = new HttpClient();
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
            var externalUrl = passThrough ? _externalExchangeRecord.PassThroughHandler.Url : _externalExchangeRecord.ExternalExchangeHandler.Url;

            var externalResponse = await Utils.EfficientApiCalls.HttpClientHelpers.PostStreamAsync(
                externalUrl,
                headers,
                new TokenExchangeRequestPackage(tokenExchangeRequest)
                {
                    MapOpaqueKeyValuePairs = mapOpaqueKeyValuePairs
                },
                CancellationToken.None);
            if (externalResponse.statusCode == HttpStatusCode.OK)
            {
                if (passThrough)
                {
                    var passThroughResult = JsonConvert.DeserializeObject<List<TokenExchangeResponse>>(externalResponse.content);
                    return passThroughResult;
                }
                else
                {
                    var tokenExchangeResponses = new List<TokenExchangeResponse>();
                    var resourceOwnerTokenRequests = JsonConvert.DeserializeObject<List<ResourceOwnerTokenRequest>>(externalResponse.content);

                    foreach (var resourceOwnerTokenRequest in resourceOwnerTokenRequests)
                    {
                        resourceOwnerTokenRequest.ClientId = _externalExchangeRecord.ExternalExchangeHandler.ClientId;
                        var response = await _tokenMintingService.MintResourceOwnerTokenAsync(resourceOwnerTokenRequest);
                        if (response.IsError)
                        {
                            throw new Exception(response.Error);
                        }

                        var tokenExchangeResponse = new TokenExchangeResponse()
                        {
                            access_token = response.AccessToken,
                            refresh_token = response.RefreshToken,
                            expires_in = response.ExpiresIn,
                            token_type = response.TokenType,
                            authority = $"{_httpContextAssessor.HttpContext.Request.Scheme}://{_httpContextAssessor.HttpContext.Request.Host}",
                            HttpHeaders = new List<HttpHeader>
                            {
                                new HttpHeader() {Name = "x-authScheme", Value = response.Scheme}
                            }
                        };
                        tokenExchangeResponses.Add(tokenExchangeResponse);
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