using AppIdentity.Models;
using AppIdentity.Query;
using Microsoft.Extensions.DependencyInjection;
using P7Core.GraphQLCore;


namespace AppIdentity.Extensions
{
    public static class AspNetCoreServiceExtensions
    {
        public static void AddGraphQLAppIdentityTypes(this IServiceCollection services)
        {

            //Application Identity Bind Query

            services.AddTransient<AppIdentityBindInput>();
            services.AddTransient<AppIdentityResultType>();
            services.AddTransient<AppIdentityRefreshInput>();
            
            services.AddTransient<IQueryFieldRegistration, AppIdentityBindQuery>();

        }
    }
}
