using System;
using System.Collections.Generic;
using System.Text;
using CustomerLoyalyStore.GraphQL.Models;
using GraphQL;
using P7.GraphQLCore;

namespace CustomerLoyalyStore.GraphQL.Query
{
    public class AuthRequiredQuery : IQueryFieldRecordRegistration
    {
        public AuthRequiredQuery()
        {
        }

        public void AddGraphTypeFields(QueryCore queryCore)
        {
            queryCore.FieldAsync<IdentityModelType>(name: "authRequired",
                description: null,
                resolve: async context =>
                {
                    try
                    {
                        var userContext = context.UserContext.As<GraphQLUserContext>();
                        var result = new Models.IdentityModel {Claims = new List<ClaimModel>()};
                        foreach (var claim in userContext.HttpContextAccessor.HttpContext.User.Claims)
                        {
                            result.Claims.Add(new ClaimModel()
                            {
                                Name = claim.Type,
                                Value = claim.Value
                            });
                        }

                        return result;
                    }
                    catch (Exception e)
                    {

                    }

                    return null;
                    //                    return await Task.Run(() => { return ""; });
                },
                deprecationReason: null);
        }
    }
}
