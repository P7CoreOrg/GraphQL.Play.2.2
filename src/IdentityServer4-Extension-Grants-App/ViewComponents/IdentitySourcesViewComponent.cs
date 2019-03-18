using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityModelExtras;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer4_Extension_Grants_App.ViewComponents
{
    public class IdentitySourcesViewComponent : ViewComponent
    {
        private ConfiguredDiscoverCacheContainerFactory _configuredDiscoverCacheContainerFactory;


        public IdentitySourcesViewComponent(
            ConfiguredDiscoverCacheContainerFactory configuredDiscoverCacheContainerFactory)
        {
            _configuredDiscoverCacheContainerFactory = configuredDiscoverCacheContainerFactory;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var sources = _configuredDiscoverCacheContainerFactory.GetAll();
            return View(sources);
        }
    }
}
