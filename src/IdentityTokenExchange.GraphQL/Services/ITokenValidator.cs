using System.Security.Claims;
using System.Threading.Tasks;
using IdentityTokenExchange.GraphQL.Query;

namespace IdentityTokenExchange.GraphQL.Services
{
    public interface ITokenValidator
    {
        Task<ClaimsPrincipal> ValidateTokenAsync(TokenDescriptor tokenDescriptor);
    }
}