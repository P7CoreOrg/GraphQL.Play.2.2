﻿using System.Linq;
using System.Threading.Tasks;
using IdentityModel.Client;
using GraphQLPlay.IdentityModelExtras;
using Microsoft.AspNetCore.Mvc;

namespace GraphQLPlayTokenExchangeOnlyApp.ViewComponents
{
    public class IdentitySourceViewComponentModel
    {
        public DiscoveryResponse DiscoveryResponse { get; set; }
        public string SchemeId { get; set; }
    }
    public class IdentitySourceViewComponent : ViewComponent
    {
        private DiscoverCacheContainerFactory _discoverCacheContainerFactory;


        public IdentitySourceViewComponent(
            DiscoverCacheContainerFactory discoverCacheContainerFactory)
        {
            _discoverCacheContainerFactory = discoverCacheContainerFactory;
        }

        public async Task<IViewComponentResult> InvokeAsync(string id)
        {
            string sourceId = id;
            if (string.IsNullOrWhiteSpace(sourceId))
            {
                sourceId = _discoverCacheContainerFactory.GetAll().Keys.FirstOrDefault();
            }

            var discoveryResponse = await _discoverCacheContainerFactory.Get(sourceId).DiscoveryCache.GetAsync();
            var model = new IdentitySourceViewComponentModel()
            {
                SchemeId = sourceId,
                DiscoveryResponse = discoveryResponse
            };

            return View(model);
        }
    }
}