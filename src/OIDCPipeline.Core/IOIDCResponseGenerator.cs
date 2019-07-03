using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace OIDCPipeline.Core
{
    public interface IOIDCResponseGenerator
    {
        Task<IActionResult> CreateIdTokenActionResultResponseAsync(string key, bool delete);
    }
}
