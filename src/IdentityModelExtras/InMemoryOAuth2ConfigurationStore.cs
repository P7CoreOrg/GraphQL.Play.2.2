using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace IdentityModelExtras
{
    
    public class InMemoryOAuth2ConfigurationStore : IOAuth2ConfigurationStore
    {
        class Oauth2Section
        {
            public List<WellknownAuthority> Authorities { get; set; }
        }
        private IConfiguration _configuration;
        private ILogger<InMemoryOAuth2ConfigurationStore> _logger;
        private Oauth2Section _oAuth2Section;

        public InMemoryOAuth2ConfigurationStore(
            IConfiguration configuration,
            ILogger<InMemoryOAuth2ConfigurationStore> logger)
        {
            _configuration = configuration;
            _logger = logger;
            var section = configuration.GetSection("InMemoryOAuth2ConfigurationStore:oauth2");
            _oAuth2Section = new Oauth2Section();
            section.Bind(_oAuth2Section);
        }

        public Task<WellknownAuthority> GetWellknownAuthorityAsync(string scheme)
        {
            var result = from item in _oAuth2Section.Authorities
                where item.Scheme == scheme
                select item;
            return Task.FromResult(result.FirstOrDefault());

        }

        public Task<List<WellknownAuthority>> GetWellknownAuthoritiesAsync()
        {
            return Task.FromResult(_oAuth2Section.Authorities);
        }
    }
}
