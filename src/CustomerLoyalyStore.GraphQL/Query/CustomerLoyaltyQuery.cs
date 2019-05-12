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
using P7Core.GraphQLCore;
using Utils;

namespace CustomerLoyalyStore.GraphQL.Query
{
    public static class StringExtenstions
    {
        public static bool EqualsNoCase(this string str1, string str2) =>
            string.Equals(str1, str2, StringComparison.InvariantCultureIgnoreCase);
    }

    public class CustomerLoyaltyQuery : IQueryFieldRegistration
    {
        private readonly ICustomerLoyaltyStore _customerLoyaltyStore;
        private readonly IPrizeStore _lazyPrizeStore;

        public CustomerLoyaltyQuery(ICustomerLoyaltyStore customerLoyaltyStore,
            IPrizeStore lazyPrizeStore)
        {
            _customerLoyaltyStore = customerLoyaltyStore;
            _lazyPrizeStore = lazyPrizeStore;
        }

        public void AddGraphTypeFields(QueryCore queryCore)
        {
            queryCore.FieldAsync<CustomerResultType>(name: "customerLoyalty",
                description: null,
                resolve: async context =>
                {

                    var startQuery = context?.Operation?
                        .SelectionSet?
                        .Selections
                        .Select(x => x as Field)
                        .FirstOrDefault();


                    var fields = startQuery?.SelectionSet?
                        .Selections
                        .Select(x => x as Field)
                        .ToList();
                    var prizesField = fields?
                        .Where(x => "prizes".EqualsNoCase(x.Name))
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
                            var prizeStore = _lazyPrizeStore;
                            var prizes =
                                await prizeStore.GetAvailablePrizesAsync(customer.LoyaltyPointBalance);
                            customerResult.Prizes = prizes;
                        }

                        return customerResult;
                    }
                    return null;
                },
                deprecationReason: null);
        }
    }
}
