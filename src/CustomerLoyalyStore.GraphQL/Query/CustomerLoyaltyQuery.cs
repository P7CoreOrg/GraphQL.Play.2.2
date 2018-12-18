using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using CustomerLoyaltyStore;
using CustomerLoyaltyStore.Models;
using CustomerLoyalyStore.GraphQL;
using CustomerLoyalyStore.GraphQL.Query;
using GraphQL;
using GraphQL.Language.AST;
using GraphQL.Types;
using P7.GraphQLCore;

namespace CustomerLoyalyStore.GraphQL.Query
{
    public static class StringExtenstions
    {
        public static bool EqualsNoCase(this string str1, string str2) =>
            string.Equals(str1, str2, StringComparison.InvariantCultureIgnoreCase);
    }
    public class CustomerLoyaltyQuery : IQueryFieldRecordRegistration
    {
        private ICustomerLoyaltyStore _customerLoyaltyStore;

        public CustomerLoyaltyQuery(ICustomerLoyaltyStore customerLoyaltyStore)
        {
            _customerLoyaltyStore = customerLoyaltyStore;
        }

        public void AddGraphTypeFields(QueryCore queryCore)
        {
            queryCore.FieldAsync<CustomerResultType>(name: "customerLoyalty",
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
                        var prizesField = startQuery?.SelectionSet?
                            .Selections
                            .Select(x => x as Field)
                            .Where(x => x != null && "prizes".EqualsNoCase(x.Name))
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
                            var customerResult = new CustomerResult
                            {
                                ID = customer.ID,
                                LoyaltyPointBalance = customer.LoyaltyPointBalance
                                
                            };

                            if (prizesField != null)
                            {
                                var prizes =
                                    await _customerLoyaltyStore.GetAvailablePrizesAsync(customer.LoyaltyPointBalance);
                                customerResult.Prizes = prizes;
                            }
                            return customerResult;
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
