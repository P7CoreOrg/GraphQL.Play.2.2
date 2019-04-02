using System.Collections.Generic;

namespace IdentityTokenExchangeGraphQL.Models
{
    public class IdentityModel
    {
        public List<ClaimModel> Claims { get; set; }
    }
}