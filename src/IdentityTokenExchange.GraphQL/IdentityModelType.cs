using GraphQL.Types;

namespace IdentityTokenExchangeGraphQL
{
    public class IdentityModelType : ObjectGraphType<Models.IdentityModel>
    {
        public IdentityModelType()
        {
            Name = "identity";
            Field<ListGraphType<ClaimModelType>>("claims", "The Claims of the identity");
        }
    }
}