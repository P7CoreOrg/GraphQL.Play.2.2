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
        public static IServiceCollection AddLazier(this IServiceCollection services)
        {
            services.AddTransient(typeof(LazyService<>));
            return services;
        }
        public static IApplicationBuilder UseLowercaseRewriter(this IApplicationBuilder app )
        {
            app.UseRewriter(new RewriteOptions().Add(new RewriteLowerCaseRule()));
            return app;
        }
    }
}
