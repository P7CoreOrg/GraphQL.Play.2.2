using System.Security.Claims;
using System.Threading.Tasks;

namespace TokenExchange.Contracts
{
    public interface ISelfValidator
    {
        Task<ClaimsPrincipal> ValidateTokenAsync(string token);
    }
}