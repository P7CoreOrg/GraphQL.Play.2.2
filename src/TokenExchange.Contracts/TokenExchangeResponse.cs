using System.Collections.Generic;
using TokenExchange.Contracts.Models;
using Utils.Models;

namespace TokenExchange.Contracts
{
    public class TokenExchangeResponse
    {
        public string access_token { get; set; }
        public int expires_in { get; set; }
        public string token_type { get; set; }
        public string refresh_token { get; set; }
        public string authority { get; set; }
        public List<HttpHeader> HttpHeaders { get; set; }
    }
}