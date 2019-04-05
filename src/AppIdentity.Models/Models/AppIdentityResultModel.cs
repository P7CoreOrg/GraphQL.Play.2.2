using System;

namespace AppIdentity.Models
{
    public class AppIdentityResultModel
    {
        public string id_token { get; set; }
        public int expires_in { get; set; }
        public string authority { get; set; }

    }


}