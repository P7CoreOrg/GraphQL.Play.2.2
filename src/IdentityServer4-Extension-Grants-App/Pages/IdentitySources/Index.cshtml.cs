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
        private DiscoverCacheContainerFactory _discoverCacheContainerFactory;

        public IndexModel(
            DiscoverCacheContainerFactory discoverCacheContainerFactory
        )
        {
            _discoverCacheContainerFactory = discoverCacheContainerFactory;
        }
        public async Task OnGetAsync(string sourceId)
        {
            SourceId = sourceId;
        }
    }
}