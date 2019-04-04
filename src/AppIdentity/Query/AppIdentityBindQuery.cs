using AppIdentity.Models;
using GraphQL;
using GraphQL.Types;
using P7Core.GraphQLCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppIdentity.Query
{
    public class AppIdentityBindQuery : IQueryFieldRegistration
    {
        public AppIdentityBindQuery()
        {

        }
        public void AddGraphTypeFields(QueryCore queryCore)
        {
            queryCore.FieldAsync<AppIdentityResultType>(name: "appIdentityBind",
                description: $"Issues an application identity.",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<AppIdentityBindInput>> { Name = "input" }),
                resolve: async context =>
                {
                    try
                    {
                        var input = context.GetArgument<AppIdentityBindInputModel>("input");


                        var bindResult = new AppIdentityResultModel
                        {
                            authority ="blah",
                            expires_in = 1234,
                            id_token = "id_token_blah"
                        };
                        return bindResult;
                    }
                    catch (Exception e)
                    {
                        context.Errors.Add(new ExecutionError("Unable to process request", e));
                    }

                    return null;
                },
                deprecationReason: null);
        }
    }
}
