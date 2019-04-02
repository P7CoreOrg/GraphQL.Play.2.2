using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityModelExtras
{
    public interface IOAuth2ConfigurationStore
    {
        Task<WellknownAuthority> GetWellknownAuthorityAsync(string scheme);
        Task<List<WellknownAuthority>> GetWellknownAuthoritiesAsync();
    }
}