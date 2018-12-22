using Microsoft.Extensions.DependencyInjection;
using Orders.Mutation;
using Orders.Query;
using Orders.Schema;
using Orders.Services;
using P7.GraphQLCore;

namespace Orders.Extensions
{
    public static class AspNetCoreServiceExtensions
    {
        public static void AddGraphQLOrders(this IServiceCollection services)
        {
            services.AddTransient<CustomerType>();
            services.AddTransient<OrderCreateInputType>();
            services.AddTransient<OrderEventType>();
            services.AddTransient<OrdersSubscription>();
            services.AddTransient<OrderStatusesEnum>();
            services.AddTransient<OrderType>();

            services.AddTransient<IMutationFieldRecordRegistration, OrdersMutation>();
            services.AddTransient<IQueryFieldRecordRegistration, OrdersQuery>();

            services.AddSingleton<ICustomerService, CustomerService>();
            services.AddSingleton<IOrderEventService, OrderEventService>();
            services.AddSingleton<IOrderService, OrderService>();
        }
    }
}