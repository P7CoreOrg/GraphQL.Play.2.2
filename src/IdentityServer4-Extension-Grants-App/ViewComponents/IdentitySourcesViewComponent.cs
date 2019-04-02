﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityModelExtras;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer4_Extension_Grants_App.ViewComponents
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
