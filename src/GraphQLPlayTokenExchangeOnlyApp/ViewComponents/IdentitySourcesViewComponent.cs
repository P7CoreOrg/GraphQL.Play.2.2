using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQLPlay.IdentityModelExtras;
using Microsoft.AspNetCore.Mvc;

namespace GraphQLPlayTokenExchangeOnlyApp.ViewComponents
{
    public class IdentitySourcesViewComponent : ViewComponent
    {
        private DiscoverCacheContainerFactory _discoverCacheContainerFactory;


        public IdentitySourcesViewComponent(
            DiscoverCacheContainerFactory discoverCacheContainerFactory)
        {
            _discoverCacheContainerFactory = discoverCacheContainerFactory;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var sources = _discoverCacheContainerFactory.GetAll();
            return View(sources);
        }
    }
}
