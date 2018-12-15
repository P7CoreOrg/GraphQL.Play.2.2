using System.Collections.Generic;

namespace IdentityTokenExchange.GraphQL.Models
{
    public class IdentityModel
    {
        public List<ClaimModel> Claims { get; set; }
    }
}