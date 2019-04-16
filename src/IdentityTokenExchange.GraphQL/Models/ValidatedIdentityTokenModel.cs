using System.Security.Claims;

namespace IdentityTokenExchangeGraphQL.Models
{
    public class ValidatedIdentityTokenModel: IdentityTokenModel
    {
        public ClaimsPrincipal Principal { get; set; }
    }
}