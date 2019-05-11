using GraphQL.Types;

namespace UnitTestApiCollection.Models
{
    public class UnitTestResultType : ObjectGraphType<UnitTestResult>
    {
        public UnitTestResultType()
        {
            Name = "unitTestResult";
            Field<ListGraphType<ComplexDataType>>("collection", "collection or complex data");
        }
    }
}