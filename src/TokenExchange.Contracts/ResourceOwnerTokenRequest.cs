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

    public enum TokenType
    {
        TokenType_IdentityToken = 1,
        TokenType_AccessToken = 2
    }



    public class ArbitraryResourceOwnerTokenRequest
    {
        public string Hint { get; set; }
        public string Scope { get; set; }
        public Dictionary<string, List<string>> ArbitraryClaims { get; set; }
        public string Subject { get; set; }
        public int AccessTokenLifetime { get; set; }
        public List<HttpHeader> HttpHeaders { get; set; }

    }

    public class ArbitraryIdentityTokenRequest
    {
        public string Hint { get; set; }
        public string Scope { get; set; }
        public Dictionary<string, List<string>> ArbitraryClaims { get; set; }
        public string Subject { get; set; }
        public int IdentityTokenLifetime { get; set; }
        public List<HttpHeader> HttpHeaders { get; set; }

    }
    public class ExternalExchangeTokenResponse
    {
        public CustomTokenResponse CustomTokenResponse { get; set; }
        public ArbitraryResourceOwnerTokenRequest ArbitraryResourceOwnerTokenRequest { get; set; }
        public ArbitraryIdentityTokenRequest ArbitraryIdentityTokenRequest { get; set; }
    }
}