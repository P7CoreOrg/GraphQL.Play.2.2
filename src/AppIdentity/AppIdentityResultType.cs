using AppIdentity.Models;
using GraphQL.Types;


namespace AppIdentity
{
    public class AppIdentityResultType : ObjectGraphType<AppIdentityResultModel>
    {
        public AppIdentityResultType()
        {
            Name = "appIdentityResult";
            Field<StringGraphType>("id_token", "identity");
            Field<IntGraphType>("expires_in", "expired_in");
            Field<StringGraphType>("authority", "The issuing authority of the identity");
        }
    }
}