using Microsoft.Extensions.DependencyInjection;

namespace CustomerLoyaltyStore.Extensions
{
    public static class AspNetCoreServiceExtensions
    {
        public static void AddCustomerLoyalty(this IServiceCollection services)
        {
            services.AddSingleton<ICustomerLoyaltyStore,CustomerLoyaltyStore>();
            services.AddSingleton<IPrizeStore, PrizeStore>();
        }
    }
} 