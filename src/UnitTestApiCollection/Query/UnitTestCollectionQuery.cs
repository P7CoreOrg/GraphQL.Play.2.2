using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GraphQL.Types;
using P7Core.GraphQLCore;
using UnitTestApiCollection.Models;

namespace UnitTestApiCollection.Query
{
    public class UnitTestCollectionQuery : IQueryFieldRegistration
    {
        public UnitTestCollectionQuery()
        {
        }

        public void AddGraphTypeFields(QueryCore queryCore)
        {
            queryCore.FieldAsync<UnitTestResultType>(name: "unitTestCollection",
                description: $"a set of complex data collections",
                resolve: UnitTestCollectionResolver,
                deprecationReason: null);
            queryCore.FieldAsync<UnitTestResultType>(name: "unitTestCollectionThrow",
                description: $"THROWS a set of complex data collections",
                resolve: UnitTestCollectionResolverThrows,
                deprecationReason: null);
        }
        internal async Task<object> UnitTestCollectionResolverThrows(ResolveFieldContext<object> context)
        {
            throw new Exception("Throws on purpose!");
        }

        internal async Task<object> UnitTestCollectionResolver(ResolveFieldContext<object> context)
        {

            return new UnitTestResult()
            {
                Collection = new List<ComplexData>()
                {
                    new ComplexData()
                    {
                        Age = 3,
                        Name = "Zippy McZipFace"
                    },
                    new ComplexData()
                    {
                        Age = 87,
                        Name = "Zarg Von ZargSon"
                    }
                }
            };
        }

    }
}
