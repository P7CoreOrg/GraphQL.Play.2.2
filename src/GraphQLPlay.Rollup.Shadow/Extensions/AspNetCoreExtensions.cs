using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace GraphQLPlay.Rollup.Shadow
{
    public static class AspNetCoreExtensions
    {
        public static IServiceCollection AddGraphQLPlayRollup(this IServiceCollection services, IConfiguration configuration)
        {
            return services;
        }
    }
}
