using Microsoft.Extensions.DependencyInjection;
using P7Core.BurnerGraphQL2.Schema;
using P7Core.GraphQLCore;

namespace P7Core.BurnerGraphQL2.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddBurnerGraphQL2(this IServiceCollection services)
        {
            services.AddTransient<IQueryFieldRegistration, DogQuery>();
        }
    }
}