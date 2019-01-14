using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Utils.Extensions
{
    public static class AspNetCoreServiceCollectionExtensions
    {
        public static IServiceCollection AddLazier(this IServiceCollection services)
        {
            services.AddTransient(typeof(LazyService<>));
            return services;
        }
    }
}
