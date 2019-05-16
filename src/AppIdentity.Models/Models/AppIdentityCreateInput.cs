using GraphQL.Types;
using System;

namespace AppIdentity.Models
{
    public class AppIdentityCreateInput : InputObjectGraphType
    {
        public AppIdentityCreateInput()
        {
            Name = "appIdentityCreate";
            Field<NonNullGraphType<StringGraphType>>("appId");
            Field<NonNullGraphType<StringGraphType>>("machineId");
            Field<StringGraphType>("subject"); // can be null, but if it is NOT, then we check if authenticated
        }
    }

}
