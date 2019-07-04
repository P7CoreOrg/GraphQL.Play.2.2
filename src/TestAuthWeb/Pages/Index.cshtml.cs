using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using OIDC.ReferenceWebClient.Constants;
using OIDC.ReferenceWebClient.Extensions;
using OIDC.ReferenceWebClient.Models;

namespace OIDC.ReferenceWebClient.Pages
{
    public class IndexModel : PageModel
    {
        private IMemoryCache _cache;

        public IndexModel(IMemoryCache cache)
        {
            _cache = cache;
        }
        public List<Claim> Claims { get; set; }
        public OpenIdConnectSessionDetails OpenIdConnectSessionDetails { get; set; }
        public void OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {
                var key = this.GetJsonCookie<string>(".oidc.memoryCacheKey");

                var oidcMessage = _cache.Get<OpenIdConnectMessage>(key);

                OpenIdConnectSessionDetails = HttpContext.Session.Get<OpenIdConnectSessionDetails>(Wellknown.OIDCSessionKey);

                Claims = Request.HttpContext.User.Claims.ToList();
            }
        }
    }
}
