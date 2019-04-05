using GraphQL.Types;

namespace AppIdentity.Models
{
    public class AppIdentityRefreshInput : InputObjectGraphType
    {
        public AppIdentityRefreshInput()
        {
            Name = "appIdentityRefresh";
            Field<NonNullGraphType<StringGraphType>>("id_token");
        }
    }

}
