using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using IdentityModel.Client;
using Microsoft.Extensions.Configuration;

namespace IdentityModelExtras
{
    /*
     "oauth2": [
        {
          "scheme": "norton",
          "authority": "https://login-int.norton.com/sso/oidc1/token",
          "callbackPath": "/signin-norton",
          "additionalEndpointBaseAddresses": [
            "https://login-int.norton.com/sso/idp/OIDC",
            "https://login-int.norton.com/sso/oidc1"
          ]
        },  
        {
          "scheme": "google",
          "authority": "https://accounts.google.com",
          "callbackPath": "/signin-google",
          "additionalEndpointBaseAddresses": []
        }
      ]
   */

    public class OAuth2SchemeRecord
    {
        public string Scheme { get; set; }
        public string ClientId { get; set; }
        public string Authority { get; set; }
        public string CallbackPath { get; set; }
        public List<string> AdditionalEndpointBaseAddresses { get; set; }
    }

    public class ConfiguredDiscoverCacheContainer : IDiscoveryCacheContainer
    {
        private readonly IDefaultHttpClientFactory _defaultHttpClientFactory;
        private WellknownAuthority _wellknownAuthority;
        private DiscoveryCache _discoveryCache { get; set; }
 
        

        public ConfiguredDiscoverCacheContainer(IDefaultHttpClientFactory defaultHttpClientFactory, 
            WellknownAuthority wellknownAuthority)
        {
            _defaultHttpClientFactory = defaultHttpClientFactory;
            _wellknownAuthority = wellknownAuthority;
        }

        public  DiscoveryCache DiscoveryCache
        {
            get
            {
                if (_discoveryCache == null)
                {
                     

                    DiscoveryPolicy discoveryPolicy = new DiscoveryPolicy()
                    {
                        ValidateIssuerName = false,
                        ValidateEndpoints = false,
                    };
                    if (_wellknownAuthority.AdditionalEndpointBaseAddresses != null && _wellknownAuthority.AdditionalEndpointBaseAddresses.Any())
                    {
                        foreach (var additionalEndpointBaseAddress in _wellknownAuthority.AdditionalEndpointBaseAddresses)
                        {
                            discoveryPolicy.AdditionalEndpointBaseAddresses.Add(additionalEndpointBaseAddress);
                        }
                    }
                    _discoveryCache = new DiscoveryCache(
                        _wellknownAuthority.Authority, 
                        _defaultHttpClientFactory.HttpClient,
                        discoveryPolicy);
                }
                return _discoveryCache;
            }
        }
    }

    public class ConfiguredDiscoverCacheContainerFactory
    {
        private IConfiguration _configuration;
        private IDefaultHttpClientFactory _defaultHttpClientFactory;
        private IOAuth2ConfigurationStore _oAuth2ConfigurationStore;
        private Dictionary<string, ConfiguredDiscoverCacheContainer> _oIDCDiscoverCacheContainers;

        private Dictionary<string, ConfiguredDiscoverCacheContainer> OIDCDiscoverCacheContainers
        {
            get
            {
                if (_oIDCDiscoverCacheContainers == null)
                {
                    _oIDCDiscoverCacheContainers = new Dictionary<string, ConfiguredDiscoverCacheContainer>();
                    var authorities = _oAuth2ConfigurationStore.GetWellknownAuthoritiesAsync().GetAwaiter().GetResult();
                    foreach (var record in authorities)
                    {
                        _oIDCDiscoverCacheContainers.Add(record.Scheme,
                            new ConfiguredDiscoverCacheContainer(_defaultHttpClientFactory, record));
                    }
                }

                return _oIDCDiscoverCacheContainers;
            }
        }

        public ConfiguredDiscoverCacheContainerFactory(
            IDefaultHttpClientFactory defaultHttpClientFactory,
            IOAuth2ConfigurationStore oAuth2ConfigurationStore,
            IConfiguration configuration)
        {
            _defaultHttpClientFactory = defaultHttpClientFactory;
            _oAuth2ConfigurationStore = oAuth2ConfigurationStore;
        }


        public Dictionary<string, ConfiguredDiscoverCacheContainer> GetAll()
        {
            return OIDCDiscoverCacheContainers;
        }
        public ConfiguredDiscoverCacheContainer Get(string scheme)
        {
            if (OIDCDiscoverCacheContainers.ContainsKey(scheme))
            {
                return OIDCDiscoverCacheContainers[scheme];
            }
            return null;
        }
    }
}