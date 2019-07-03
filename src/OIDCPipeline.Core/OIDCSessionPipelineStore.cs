using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OIDCPipeline.Core.Extensions;
using System;
using System.Threading.Tasks;

namespace OIDCPipeline.Core
{
    public class OIDCSessionPipelineStore : IOIDCPipelineStore
    {
        IHttpContextAccessor _httpContextAccessor;
        public OIDCSessionPipelineStore(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IActionResult> CreateIdTokenActionResultResponseAsync(string key, bool delete)
        {
            if (delete)
            {
                await DeleteStoredCacheAsync(key);
            }
            throw new NotImplementedException();
        }

        public Task DeleteStoredCacheAsync(string key)
        {
            _httpContextAccessor.HttpContext.Session.Remove(GenerateOriginalIdTokenRequestKey(key));
            _httpContextAccessor.HttpContext.Session.Remove(GenerateDownstreamIdTokenResponseKey(key));
            return Task.CompletedTask;
        }
        string GenerateDownstreamIdTokenResponseKey(string key)
        {
            var hashCode = $"_oidcSession.downstream.{key}";
            return hashCode;
        }
        string GenerateOriginalIdTokenRequestKey(string key)
        {
            var hashCode = $"_oidcSession.original.{key}";
            return hashCode;
        }
        public Task StoreDownstreamIdTokenResponse(string key, IdTokenResponse response)
        {
            _httpContextAccessor.HttpContext.Session.Set(GenerateDownstreamIdTokenResponseKey(key), response);
            return Task.CompletedTask;
        }

        public Task StoreOriginalIdTokenRequestAsync(string key, IdTokenAuthorizationRequest request)
        {
            _httpContextAccessor.HttpContext.Session.Set(GenerateOriginalIdTokenRequestKey(key), request);
            return Task.CompletedTask;
        }

        public Task<IdTokenAuthorizationRequest> GetOriginalIdTokenRequestAsync(string key)
        {
           var result =  _httpContextAccessor.HttpContext.Session.Get< IdTokenAuthorizationRequest>(GenerateOriginalIdTokenRequestKey(key));
            return Task.FromResult(result);
        }

        public Task<IdTokenResponse> GetDownstreamIdTokenResponse(string key)
        {
            var result = _httpContextAccessor.HttpContext.Session.Get<IdTokenResponse>(GenerateDownstreamIdTokenResponseKey(key));
            return Task.FromResult(result);
        }
    }
}
