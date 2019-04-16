using System;
using System.Collections.Generic;

namespace IdentityTokenExchangeGraphQL.Models
{

    public class BindInputModel 
    {
        /// <summary>
        /// A wellknown lookup used to validate the token
        /// </summary>
        public string Exchange { get; set; }

        public List<string> Extras { get; set; }
        public List<IdentityTokenModel> Tokens { get; set; }
    }
}