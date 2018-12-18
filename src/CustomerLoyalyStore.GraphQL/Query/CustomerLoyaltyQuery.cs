using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using CustomerLoyaltyStore;
using CustomerLoyalyStore.GraphQL;
using CustomerLoyalyStore.GraphQL.Query;
using GraphQL;
using GraphQL.Language.AST;
using GraphQL.Types;
using P7.GraphQLCore;

namespace CustomerLoyalyStore.GraphQL.Query
{

    public class CustomerLoyaltyQuery : IQueryFieldRecordRegistration
    {
        private ICustomerLoyaltyStore _customerLoyaltyStore;

        public CustomerLoyaltyQuery(ICustomerLoyaltyStore customerLoyaltyStore)
        {
            _customerLoyaltyStore = customerLoyaltyStore;
        }

        public void AddGraphTypeFields(QueryCore queryCore)
        {
            queryCore.FieldAsync<CustomerType>(name: "customerLoyalty",
                description: null,
                resolve: async context =>
                {
                    try
                    {
                        var startQuery = context?.Operation?
                            .SelectionSet?
                            .Selections
                            .Select(x => x as Field)
                            .FirstOrDefault();


                        var userContext = context.UserContext.As<GraphQLUserContext>();
                        var user = userContext.HttpContextAccessor.HttpContext.User;
                        var query = from item in user.Claims
                            where item.Type == ClaimTypes.NameIdentifier
                            select item;
                        if (query.Any())
                        {
                            var claim = query.First();
                            var userId = claim.Value;
                            var customer = await _customerLoyaltyStore.GetCustomerAsync(userId);
                            return customer;
                        }

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
