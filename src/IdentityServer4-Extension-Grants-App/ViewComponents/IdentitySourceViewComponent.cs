using System.Linq;
using System.Threading.Tasks;
using IdentityModel.Client;
using IdentityModelExtras;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer4_Extension_Grants_App.ViewComponents
{
    public class IdentitySourceViewComponentModel
    {
        public DiscoveryResponse DiscoveryResponse { get; set; }
    }
    public class IdentitySourceViewComponent : ViewComponent
    {
        private ConfiguredDiscoverCacheContainerFactory _configuredDiscoverCacheContainerFactory;


        public IdentitySourceViewComponent(
            ConfiguredDiscoverCacheContainerFactory configuredDiscoverCacheContainerFactory)
        {
            _configuredDiscoverCacheContainerFactory = configuredDiscoverCacheContainerFactory;
        }

        public async Task<IViewComponentResult> InvokeAsync(string id)
        {
            string sourceId = id;
            if (string.IsNullOrWhiteSpace(sourceId))
            {
                sourceId = _configuredDiscoverCacheContainerFactory.GetAll().Keys.FirstOrDefault();
            }

            var discoveryResponse = await _configuredDiscoverCacheContainerFactory.Get(sourceId).DiscoveryCache.GetAsync();
            var model = new IdentitySourceViewComponentModel()
            {
                DiscoveryResponse = discoveryResponse
            };
          
            return View(model);
        }
    }
}