using System.Collections.Generic;

namespace TokenExchange.Contracts
{
    public class TokenExchangeRequest
    {
        public List<TokenWithScheme> Tokens { get; set; }
        public List<string> Extras { get; set; }
    }
}