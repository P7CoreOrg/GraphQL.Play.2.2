using System.Collections.Generic;

namespace TokenExchange.Contracts
{

    public class TokenExchangeRequest
    {
        public List<ValidatedToken> ValidatedTokens { get; set; }
        public List<string> Extras { get; set; }
    }
    public class ExternalTokenExchangeRequest
    {
        public List<TokenWithScheme> Tokens { get; set; }
        public List<string> Extras { get; set; }
    }
}