using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using GraphQLPlay.Contracts;
using IdentityModel.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using TokenExchange.Contracts.Models;
using Utils.Models;

namespace TokenExchange.Contracts
{
    public class ExternalExchangePrincipalEvaluator : IPrincipalEvaluator
    {

        private IHttpContextAccessor _httpContextAssessor;
       
        private ISummaryLogger _summaryLogger;
        private object _cacheKey;
        private IOptionsSnapshot<TokenClientOptions> _optionsSnapshot;
        private IMemoryCache _memoryCache;
        private ExternalExchangeRecord _externalExchangeRecord;
        private TokenClientOptions _settings;
        private string _name;
        private IDiscoveryCache _discoveryCache;
      

        public ExternalExchangePrincipalEvaluator(
            IOptionsSnapshot<TokenClientOptions> optionsSnapshot,
            IMemoryCache memoryCache,
            IHttpContextAccessor httpContextAssessor,
            ISummaryLogger summaryLogger
            )
        {
            _cacheKey = "a9c4c7b7-dbb1-4d24-a78d-b8f89cc9ca83";
            _optionsSnapshot = optionsSnapshot;
            _memoryCache = memoryCache;
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
        public void Configure(ExternalExchangeRecord externalExchangeRecord)
        {
            _externalExchangeRecord = externalExchangeRecord;
            _settings = _optionsSnapshot.Get(_externalExchangeRecord.ExchangeName);
            _cacheKey = $"{_externalExchangeRecord.ExchangeName}_{_cacheKey}";


        }
        public string Name => _externalExchangeRecord.ExchangeName;

        public async Task<List<TokenExchangeResponse>> ProcessExchangeAsync(TokenExchangeRequest tokenExchangeRequest)
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
             
          
            var externalResponse = await Utils.EfficientApiCalls.HttpClientHelpers.PostStreamAsync(
                _externalExchangeRecord.PassThroughMint.ExchangeUrl,
                headers, tokenExchangeRequest, CancellationToken.None);
            if (externalResponse.statusCode == HttpStatusCode.OK)
            {
                var finalResult = JsonConvert.DeserializeObject<List<TokenExchangeResponse>>(externalResponse.content);
                return finalResult;
            }

            return null;
        }

        public static void RegisterServices(Microsoft.Extensions.DependencyInjection.IServiceCollection services, IExternalExchangeStore tempExternalExchangeStore)
        {
            foreach (var exchange in tempExternalExchangeStore.GetExternalExchangeRecordAsync().GetAwaiter().GetResult())
            {
                services.Configure<TokenClientOptions>(exchange.ExchangeName,options =>
                {
                    options.Authority = exchange.oAuth2_client_credentials.Authority;
                    options.ClientId = exchange.oAuth2_client_credentials.ClientId;
                    options.ClientSecret = exchange.oAuth2_client_credentials.ClientSecret;
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