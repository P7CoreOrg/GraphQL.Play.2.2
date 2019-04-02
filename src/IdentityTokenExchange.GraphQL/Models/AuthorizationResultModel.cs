using System.Collections.Generic;

namespace IdentityTokenExchangeGraphQL.Models
{
    public class AuthorizationResultModel 
    {
        public string access_token { get; set; }
        public int expires_in { get; set; }
        public string token_type { get; set; }
        public string refresh_token { get; set; }
        public string authority { get; set; }
        public List<HttpHeader> HttpHeaders { get; set; }
        public override string ToString()
        {
            return
                $"Authorization: [access_token={access_token}, expires_in={expires_in}, token_type={token_type}, refresh_token={refresh_token}, authority={authority}, httpHeaders={HttpHeaders}";
        }
    }
}