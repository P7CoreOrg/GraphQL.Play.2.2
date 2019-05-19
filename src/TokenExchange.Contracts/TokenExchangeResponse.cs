using System.Collections.Generic;
using TokenExchange.Contracts.Models;
using Utils.Models;

namespace TokenExchange.Contracts
{

    public class CustomTokenResponse
    {
        public string hint { get; set; }
        public string Type { get; set; }
        public string Token { get; set; }
        public string authority { get; set; }
        public List<HttpHeader> HttpHeaders { get; set; }
    }
    public class AccessTokenResponse
    {
        public string hint { get; set; }
        public string access_token { get; set; }
        public int expires_in { get; set; }
        public string token_type { get; set; }
        public string refresh_token { get; set; }
        public string authority { get; set; }
        public List<HttpHeader> HttpHeaders { get; set; }
    }
    public class IdentityTokenResponse
    {
        public string hint { get; set; }
        public string id_token { get; set; }
        public int expires_in { get; set; }
        public string authority { get; set; }
        public List<HttpHeader> HttpHeaders { get; set; }
    }
    public class TokenExchangeResponse
    {
        public AccessTokenResponse accessToken { get; set; }
        public IdentityTokenResponse IdentityToken { get; set; }
        public CustomTokenResponse customToken { get; set; }
    }
}