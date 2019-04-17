using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using GraphQLPlay.Contracts;
using IdentityModel.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TokenExchange.Contracts.Models;

namespace TokenExchange.Contracts
{
    public class ExternalExchangePrincipalEvaluator : IPrincipalEvaluator
    {

        private IHttpContextAccessor _httpContextAssessor;
       
        private ISummaryLogger _summaryLogger;
        private object _cacheKey;
        private IOptionsSnapshot<TokenClientOptions> _optionsSnapshot;
        private IMemoryCache _memoryCache;
        private IServiceProvider _serviceProvider;
        private ExternalExchangeClientCredentials _externalExchangeClientCredentials;
        private TokenClientOptions _settings;
        private string _name;
        private IDiscoveryCache _discoveryCache;
      

        public ExternalExchangePrincipalEvaluator(
            IOptionsSnapshot<TokenClientOptions> optionsSnapshot,
            IMemoryCache memoryCache,
            IServiceProvider serviceProvider,
            IHttpContextAccessor httpContextAssessor,
            ISummaryLogger summaryLogger
            )
        {
            _cacheKey = "a9c4c7b7-dbb1-4d24-a78d-b8f89cc9ca83";
            _optionsSnapshot = optionsSnapshot;
            _memoryCache = memoryCache;
            _serviceProvider = serviceProvider;
            _httpContextAssessor = httpContextAssessor;
             
            _summaryLogger = summaryLogger;
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
        public void Configure(ExternalExchangeClientCredentials externalExchangeClientCredentials)
        {
            _externalExchangeClientCredentials = externalExchangeClientCredentials;
            _settings = _optionsSnapshot.Get(_externalExchangeClientCredentials.ExchangeName);
            _cacheKey = $"{_externalExchangeClientCredentials.ExchangeName}_{_cacheKey}";


        }
        public string Name => _externalExchangeClientCredentials.ExchangeName;

        public async Task<List<TokenExchangeResponse>> ProcessExchangeAsync(TokenExchangeRequest tokenExchangeRequest)
        {
            var access_token = await GetTokenAsync();

            if (tokenExchangeRequest.Extras == null || tokenExchangeRequest.Extras.Count == 0)
            {
                throw new Exception($"{Name}: We require that extras be populated!");
            }

            // for this demo, lets assume all the extras are roles.
            var response = new List<TokenExchangeResponse>();
            for (int i = 0; i < 2; i++)
            {
                var tokenExchangeResponse = new TokenExchangeResponse()
                {
                    access_token = $"Alien_access_token_{access_token}",
                    refresh_token = $"Alien_refresh_token_{Guid.NewGuid().ToString()}",
                    expires_in = 1234+i,
                    token_type = $"Alien_{Guid.NewGuid().ToString()}",
                    authority = $"{_httpContextAssessor.HttpContext.Request.Scheme}://{_httpContextAssessor.HttpContext.Request.Host}/Alien",
                    HttpHeaders = new List<HttpHeader>
                    {
                        new HttpHeader() {Name = "x-authScheme", Value = "Alien"}
                    }

                };
                response.Add(tokenExchangeResponse);
            }
            
            return response;
        }

        public static void RegisterServices(Microsoft.Extensions.DependencyInjection.IServiceCollection services, IExternalExchangeStore tempExternalExchangeStore)
        {
            foreach (var exchange in tempExternalExchangeStore.GetClientCredentialExchangesAsync().GetAwaiter().GetResult())
            {
                services.Configure<TokenClientOptions>(exchange.ExchangeName,options =>
                {
                    options.Authority = exchange.Authority;
                    options.ClientId = exchange.ClientId;
                    options.ClientSecret = exchange.ClientSecret;
                });
                services.AddTransient<IPrincipalEvaluator>(x =>
                {
                    var externalExchangePrincipalEvaluator = x.GetRequiredService<ExternalExchangePrincipalEvaluator>();
                    externalExchangePrincipalEvaluator.Configure(exchange);
                    return externalExchangePrincipalEvaluator;
                });
            }
        }
    }
}