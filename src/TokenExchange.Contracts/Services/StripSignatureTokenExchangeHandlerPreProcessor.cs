using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace TokenExchange.Contracts.Services
{
    public class StripSignatureTokenExchangeHandlerPreProcessor : ITokenExchangeHandlerPreProcessor
    {
        public string Name => "strip-signature";

        public Task Process(ref TokenExchangeRequest tokenExchangeRequest)
        {
            var header = new JwtHeader();
            var handler = new JwtSecurityTokenHandler();
            foreach (var tokenWithScheme in tokenExchangeRequest.Tokens)
            {
                var token = handler.ReadJwtToken(tokenWithScheme.Token);
                var unsignedSecurityToken = new JwtSecurityToken(header, token.Payload);
                tokenWithScheme.Token = handler.WriteToken(unsignedSecurityToken);
            }

            return Task.CompletedTask;
        }
    }
}