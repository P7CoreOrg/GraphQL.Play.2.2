using Microsoft.Extensions.DependencyInjection;
using P7Core.GraphQLCore;
using UnitTestApiCollection.Models;
using UnitTestApiCollection.Query;

namespace UnitTestApiCollection.Extensions
{
    public static class AspNetCoreServiceExtensions
    {
        public static void AddUnitTestApiCollection(this IServiceCollection services)
        {
            services.AddTransient<UnitTestResultType>();
            services.AddTransient<ComplexDataType>();
            services.AddTransient<IQueryFieldRegistration, UnitTestCollectionQuery>();
        }
    }
}
