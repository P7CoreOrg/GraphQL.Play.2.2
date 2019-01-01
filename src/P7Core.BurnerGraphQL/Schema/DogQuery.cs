using GraphQL.Types;
using P7Core.Burner;
using P7Core.GraphQLCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace P7Core.BurnerGraphQL.Schema
{
    public class DogQuery : IQueryFieldRegistration
    {
        private async Task<string> DogNameAsync()
        {
            var dog = new Dog();
            return dog.Name;
        }
        public void AddGraphTypeFields(QueryCore queryCore)
        {
            queryCore.FieldAsync<StringGraphType>(
               "dogName",
               resolve: async context => await DogNameAsync()
           );
        }
    }
}
