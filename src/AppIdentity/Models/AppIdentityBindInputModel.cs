using System;
using System.Collections.Generic;

namespace AppIdentity.Models
{
    public class AppIdentityBindInputModel
    {
        /// <summary>
        /// Any accepted token [id_token, etc] 
        /// </summary>
        public string AppId { get; set; }
        /// <summary>
        /// The type of token, i.e oidc
        /// </summary>
        public string MachineId { get; set; }

    }
}