using System.Collections.Generic;
using Utils.Models;

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
    public class ExternalExchangeResourceOwnerTokenRequest
    {
        public string Scope { get; set; }
        public Dictionary<string, List<string>> ArbitraryClaims { get; set; }
        public string Subject { get; set; }
        public int AccessTokenLifetime { get; set; }
        public List<HttpHeader> HttpHeaders { get; set; }
    }
}