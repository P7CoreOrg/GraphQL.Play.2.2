using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Extensions.DependencyInjection;
using OIDCPipeline.Core.AuthorizationEndpoint;

namespace OIDCPipeline.Core.Extensions
{
    public static class AspNetCoreServiceExtensions
    {
        public static void AddOIDCSessionPipelineStore(this IServiceCollection services)
        {
            services.AddTransient<IOIDCPipelineStore, OIDCSessionPipelineStore>();
        }
        public static void AddOIDCPipeline(this IServiceCollection services)
        {
            services.AddTransient<IOIDCResponseGenerator, OIDCResponseGenerator>();
            services.AddTransient<IAuthorizeRequestValidator, AuthorizeRequestValidator>();
        }

    }
}
