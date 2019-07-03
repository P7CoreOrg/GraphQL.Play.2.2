using System.Threading.Tasks;

namespace OIDCPipeline.Core
{
    public interface IOIDCPipelineStore
    {
        Task StoreOriginalIdTokenRequestAsync(string key, IdTokenAuthorizationRequest request);
        Task<IdTokenAuthorizationRequest> GetOriginalIdTokenRequestAsync(string key);
        Task StoreDownstreamIdTokenResponse(string key, IdTokenResponse response);
        Task<IdTokenResponse> GetDownstreamIdTokenResponse(string key);
        Task DeleteStoredCacheAsync(string key);
       
    }
}
