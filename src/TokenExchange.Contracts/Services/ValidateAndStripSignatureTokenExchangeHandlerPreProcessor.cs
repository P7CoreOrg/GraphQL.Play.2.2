using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace TokenExchange.Contracts.Services
{
    public class ValidateAndStripSignatureTokenExchangeHandlerPreProcessor : ITokenExchangeHandlerPreProcessor
    {
        private ITokenValidator _tokenValidator;

        public ValidateAndStripSignatureTokenExchangeHandlerPreProcessor(ITokenValidator tokenValidator)
        {
            _tokenValidator = tokenValidator;
        }
        public string Name => "validate-strip-signature";

        public Task ProcessAsync(ref TokenExchangeRequest tokenExchangeRequest)
        {
            var header = new JwtHeader();
            var handler = new JwtSecurityTokenHandler();
            foreach (var tokenWithScheme in tokenExchangeRequest.Tokens)
            {
                var principal = _tokenValidator.ValidateTokenAsync(new TokenDescriptor
                {
                    TokenScheme = tokenWithScheme.TokenScheme,
                    Token = tokenWithScheme.Token
                }).GetAwaiter().GetResult();

                var token = handler.ReadJwtToken(tokenWithScheme.Token);
                var unsignedSecurityToken = new JwtSecurityToken(header, token.Payload);
                tokenWithScheme.Token = handler.WriteToken(unsignedSecurityToken);
            }
            return Task.CompletedTask;
            
        }
    }
}

