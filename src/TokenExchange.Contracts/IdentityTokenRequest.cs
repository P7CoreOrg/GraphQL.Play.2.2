using System.Collections.Generic;

namespace TokenExchange.Contracts
{
    public class IdentityTokenRequest
    {
        public string Scope { get; set; }
        public Dictionary<string, List<string>> ArbitraryClaims { get; set; }
        public string Subject { get; set; }
        public int? AccessTokenLifetime { get; set; }
    }
}