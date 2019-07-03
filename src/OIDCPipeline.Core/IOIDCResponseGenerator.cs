using Microsoft.AspNetCore.Mvc;
using System.Collections.Specialized;
using System.Threading.Tasks;

namespace OIDCPipeline.Core
{
    public interface IOIDCResponseGenerator
    {
        Task<IActionResult> CreateIdTokenActionResultResponseAsync(string key,NameValueCollection extras = null, bool delete = true);
    }
}
