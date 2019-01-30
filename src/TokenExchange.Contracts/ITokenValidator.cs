using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TokenExchange.Contracts
{
    public interface ITokenValidator
    {
        Task<ClaimsPrincipal> ValidateTokenAsync(TokenDescriptor tokenDescriptor);
    }
}
