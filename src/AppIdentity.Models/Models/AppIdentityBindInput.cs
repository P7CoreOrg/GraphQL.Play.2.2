using GraphQL.Types;
using System;

namespace AppIdentity.Models
{
    public class AppIdentityBindInput : InputObjectGraphType
    {
        public AppIdentityBindInput()
        {
            Name = "appIdentityBind";
            Field<NonNullGraphType<StringGraphType>>("appId");
            Field<NonNullGraphType<StringGraphType>>("machineId");
        }
    }

}
