using AppIdentity.Contracts;
using AppIdentity.Models;
using AppIdentity.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;


namespace AppIdentity.Extensions
{
    public static class AspNetCoreServiceExtensions
    {
        public static void AddInMemoryAppIdentityConfiguration(this IServiceCollection services, AppIdentityConfigurationModel model)
        {
            services.TryAddSingleton<IAppIdentityConfiguration>(sp =>
            {
                return new InMemoryAppIdentityConfiguration()
                {
                    MaxAppIdLength = model.MaxAppIdLength,
                    MaxMachineIdLength = model.MaxMachineIdLength,
                    MaxSubjectLength = model.MaxSubjectLength
                };
            });
        }
    }
}
