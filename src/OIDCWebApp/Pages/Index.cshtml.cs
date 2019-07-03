using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using OIDC.ReferenceWebClient.Constants;
using OIDC.ReferenceWebClient.Controllers;
using OIDC.ReferenceWebClient.Data;
using OIDC.ReferenceWebClient.Extensions;
using OIDC.ReferenceWebClient.Models;
using OIDCPipeline.Core;

namespace OIDC.ReferenceWebClient.Pages
{
    public class IndexModel : PageModel
    {
        private SignInManager<ApplicationUser> _signInManager;
        private IOIDCResponseGenerator _oidcResponseGenerator;
        private IOIDCPipelineStore _oidcPipelineStore;

        public IndexModel(SignInManager<ApplicationUser> signInManager,
            IOIDCResponseGenerator oidcResponseGenerator, IOIDCPipelineStore oidcPipelineStore)
        {
            _signInManager = signInManager;
               _oidcResponseGenerator = oidcResponseGenerator;
            _oidcPipelineStore = oidcPipelineStore;
        }
        public List<Claim> Claims { get; set; }
        public IdTokenResponse IdTokenResponse { get; private set; }
      
        public async Task OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {
                IdTokenResponse = await _oidcPipelineStore.GetDownstreamIdTokenResponse(HttpContext.Session.GetSessionId());
      
                Claims = Request.HttpContext.User.Claims.ToList();
            }
        }
        public async Task<IActionResult> OnPostWay2(string data)
        {
            var extras = new NameValueCollection
            {
                ["prodInstance"] = Guid.NewGuid().ToString()
            };

            var result = await _oidcResponseGenerator.CreateIdTokenActionResultResponseAsync(
                HttpContext.Session.GetSessionId(), extras, true);
            await _signInManager.SignOutAsync();// we don't want our loggin hanging around
            return result;

        }
    }
}
