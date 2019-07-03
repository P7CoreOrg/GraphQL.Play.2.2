using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using OIDC.ReferenceWebClient.Constants;
using OIDC.ReferenceWebClient.Controllers;
using OIDC.ReferenceWebClient.Extensions;
using OIDC.ReferenceWebClient.Models;
using OIDCPipeline.Core;

namespace OIDC.ReferenceWebClient.Pages
{
    public class IndexModel : PageModel
    {
        
        private IOIDCResponseGenerator _oidcResponseGenerator;
        private IOIDCPipelineStore _oidcPipelineStore;

        public IndexModel(
            IOIDCResponseGenerator oidcResponseGenerator, IOIDCPipelineStore oidcPipelineStore)
        {
         
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
            var result = await _oidcResponseGenerator.CreateIdTokenActionResultResponseAsync(HttpContext.Session.GetSessionId(), true);
            return result;

        }
    }
}
