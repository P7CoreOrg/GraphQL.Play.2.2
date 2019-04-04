using System.Collections.Generic;

namespace TokenExchange.Contracts
{
    public class ResourceOwnerTokenRequest
    {
        public string Scope { get; set; }
        public Dictionary<string, List<string>> ArbitraryClaims { get; set; }
        public string Subject { get; set; }
        public int AccessTokenLifetime { get; set; }
        public string ClientId { get; set; }
    }
}