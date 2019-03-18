using Microsoft.Extensions.DependencyInjection;
using P7Core.BurnerGraphQL.Schema;
using P7Core.GraphQLCore;

namespace P7Core.BurnerGraphQL.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddBurnerGraphQL(this IServiceCollection services)
        {
            services.AddTransient<IQueryFieldRegistration, DogQuery>();
            services.AddTransient<IMutationFieldRegistration, DogMutation>();

            services.AddTransient<DogType>();
            services.AddTransient<DogInputType>();

            services.AddSingleton<IDogStore, DogStore>();
        }
    }
}