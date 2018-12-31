using System;
using System.Collections.Generic;
using System.Text;
using CustomerLoyalyStore.GraphQL.Mutation;
using CustomerLoyalyStore.GraphQL.Query;
using Microsoft.Extensions.DependencyInjection;
using P7Core.GraphQLCore;

namespace CustomerLoyalyStore.GraphQL.Extensions
{
    public static class AspNetCoreServiceExtensions
    {
        public static void AddGraphQLCoreCustomLoyaltyTypes(this IServiceCollection services)
        {
            services.AddTransient<CustomerMutationInput>();
            services.AddTransient<CustomerResultType>();
            services.AddTransient<PrizeType>();
            services.AddTransient<IMutationFieldRegistration, CustomerMutation>();

            services.AddTransient<LoyaltyPointsTransferMutationInput>();
            services.AddTransient<LoyaltyPointsTransferType>();
           
            services.AddTransient<IMutationFieldRegistration, LoyaltyPointsTransferMutation>();

            services.AddTransient<IQueryFieldRegistration, CustomerLoyaltyQuery>();

            
        }
    }
}
