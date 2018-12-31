using Microsoft.Extensions.DependencyInjection;
using Orders.Mutation;
using Orders.Query;
using Orders.Schema;
using Orders.Services;
using Orders.Subscription;
using P7Core.GraphQLCore;

namespace Orders.Extensions
{
    public static class AspNetCoreServiceExtensions
    {
        public static void AddGraphQLOrders(this IServiceCollection services)
        {
            services.AddTransient<CustomerType>();
            services.AddTransient<OrderCreateInputType>();
            services.AddTransient<OrderEventType>();
 
            services.AddTransient<OrderStatusesEnum>();
            services.AddTransient<OrderType>();

            services.AddTransient<IMutationFieldRegistration, OrdersMutation>();
            services.AddTransient<IQueryFieldRegistration, OrdersQuery>();
            services.AddTransient<ISubscriptionFieldRegistration, OrdersSubscription>();


            services.AddSingleton<ICustomerService, CustomerService>();
            services.AddSingleton<IOrderEventService, OrderEventService>();
            services.AddSingleton<IOrderService, OrderService>();
        }
    }
}