using System;
using System.Collections.Generic;
using System.Text;
using CustomerLoyalyStore.GraphQL.Mutation;
using CustomerLoyalyStore.GraphQL.Query;
using Microsoft.Extensions.DependencyInjection;
using P7.GraphQLCore;

namespace CustomerLoyalyStore.GraphQL.Extensions
{
    public static class AspNetCoreServiceExtensions
    {
        public static void AddGraphQLCoreCustomLoyaltyTypes(this IServiceCollection services)
        {
            services.AddTransient<CustomerMutationInput>();
            services.AddTransient<CustomerType>();
            services.AddTransient<IMutationFieldRecordRegistration, CustomerMutation>();

            services.AddTransient<LoyaltyPointsTransferMutationInput>();
            services.AddTransient<LoyaltyPointsTransferType>();
           
            services.AddTransient<IMutationFieldRecordRegistration, LoyaltyPointsTransferMutation>();

            services.AddTransient<IQueryFieldRecordRegistration, CustomerLoyaltyQuery>();

            
        }
    }
}
