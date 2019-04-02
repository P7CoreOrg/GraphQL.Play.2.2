using System;
using System.Collections.Generic;

namespace IdentityTokenExchangeGraphQL.Models
{
    public class BindInputModel 
    {
        /// <summary>
        /// Any accepted token [id_token, etc] 
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// The type of token, i.e oidc
        /// </summary>
        public string TokenScheme { get; set; }
        /// <summary>
        /// A wellknown lookup used to validate the token
        /// </summary>
        public string Exchange { get; set; }

        public List<string> Extras { get; set; }
    }
}