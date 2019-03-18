using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityModelExtras;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityServer4_Extension_Grants_App.Pages.IdentitySources
{
    public class IndexModel : PageModel
    {
        public string SourceId { get; private set; }
        private ConfiguredDiscoverCacheContainerFactory _configuredDiscoverCacheContainerFactory;

        public IndexModel(
            ConfiguredDiscoverCacheContainerFactory configuredDiscoverCacheContainerFactory
        )
        {
            _configuredDiscoverCacheContainerFactory = configuredDiscoverCacheContainerFactory;
        }
        public async Task OnGetAsync(string sourceId)
        {
            SourceId = sourceId;
        }
    }
}