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

    }
}
