using Microsoft.Extensions.DependencyInjection;
using Orders.Schema;
using Orders.Services;
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
           
            services.AddSingleton<ICustomerService, CustomerService>();
            services.AddSingleton<IOrderEventService, OrderEventService>();
            services.AddSingleton<IOrderService, OrderService>();
        }
    }
}