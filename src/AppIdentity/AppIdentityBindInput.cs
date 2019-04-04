using GraphQL.Types;
using System;

namespace AppIdentity
{
    public class AppIdentityBindInput : InputObjectGraphType
    {
        public AppIdentityBindInput()
        {
            Name = "appIdentityBind";
            Field<NonNullGraphType<GraphQL.Types.StringGraphType>>("appId");
            Field<NonNullGraphType<StringGraphType>>("machineId");
        }
    }

}
