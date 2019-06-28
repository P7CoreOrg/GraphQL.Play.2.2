using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OIDC.ReferenceWebClient.Constants;
using OIDC.ReferenceWebClient.Extensions;
using OIDC.ReferenceWebClient.Models;

namespace OIDC.ReferenceWebClient.Pages
{
    public class IndexModel : PageModel
    {
        public List<Claim> Claims { get; set; }
        public OpenIdConnectSessionDetails OpenIdConnectSessionDetails { get; set; }
        public void OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {

                OpenIdConnectSessionDetails = HttpContext.Session.Get<OpenIdConnectSessionDetails>(Wellknown.OIDCSessionKey);

                Claims = Request.HttpContext.User.Claims.ToList();
            }
        }
    }
}
