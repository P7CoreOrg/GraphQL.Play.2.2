using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.DependencyInjection;

namespace Utils.Extensions
{
    public static class AspNetCoreServiceCollectionExtensions
    {
        public static IApplicationBuilder UseLowercaseRewriter(this IApplicationBuilder app)
        {
            app.UseRewriter(new RewriteOptions().Add(new RewriteLowerCaseRule()));
            return app;
        }

        public static IServiceCollection AddLazyTransient<TService, TImplementation>(this IServiceCollection services)
            where TService : class
            where TImplementation : class, TService
        {
            services.AddTransient<TImplementation>();
            services.AddTransient<Lazy<TService>>(serviceProvider =>
            {

                return new Lazy<TService>(() =>
                {
                    var impl = serviceProvider.GetRequiredService<TImplementation>();
                    return impl;
                });
            });
            return services;
        }
        public static IServiceCollection AddLazySingleton<TService, TImplementation>(this IServiceCollection services)
            where TService : class
            where TImplementation : class, TService
        {
            services.AddSingleton<TImplementation>();
            services.AddSingleton<Lazy<TService>>(serviceProvider =>
            {

                return new Lazy<TService>(() =>
                {
                    var impl = serviceProvider.GetRequiredService<TImplementation>();
                    return impl;
                });
            });
            return services;
        }
        public static IServiceCollection AddLazyScoped<TService, TImplementation>(this IServiceCollection services)
            where TService : class
            where TImplementation : class, TService
        {
            services.AddScoped<TImplementation>();
            services.AddScoped<Lazy<TService>>(serviceProvider =>
            {

                return new Lazy<TService>(() =>
                {
                    var impl = serviceProvider.GetRequiredService<TImplementation>();
                    return impl;
                });
            });
            return services;
        }
    }
}
