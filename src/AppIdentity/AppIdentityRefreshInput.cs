using GraphQL.Types;

namespace AppIdentity
{
    public class AppIdentityRefreshInput : InputObjectGraphType
    {
        public AppIdentityRefreshInput()
        {
            Name = "appIdentityRefresh";
            Field<NonNullGraphType<GraphQL.Types.StringGraphType>>("id_token");
        }
    }

}
