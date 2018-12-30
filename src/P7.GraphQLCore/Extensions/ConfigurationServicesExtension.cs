using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using P7.GraphQLCore.Models;
using P7.GraphQLCore.Stores;


namespace P7.GraphQLCore.Extensions
{
    public static class ConfigurationServicesExtension
    {
        public static void RegisterGraphQLCoreConfigurationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<GraphQLAuthenticationConfig>(configuration.GetSection(GraphQLAuthenticationConfig.WellKnown_SectionName));
            services.Configure<GraphQLFieldAuthorityConfig>(configuration.GetSection(GraphQLFieldAuthorityConfig.WellKnown_SectionName));
        }
    }
}