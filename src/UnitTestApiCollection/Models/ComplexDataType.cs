
using GraphQL.Types;

namespace UnitTestApiCollection.Models
{
    public class ComplexDataType : ObjectGraphType<ComplexData>
    {
        public ComplexDataType()
        {
            Name = "complexData";
            Field<IntGraphType>("age", "the age");
            Field<StringGraphType>("name", "the name");
        }
    }
}