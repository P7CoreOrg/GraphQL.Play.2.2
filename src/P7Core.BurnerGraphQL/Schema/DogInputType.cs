using GraphQL.Types;

namespace P7Core.BurnerGraphQL.Schema
{
    public class DogInputType : InputObjectGraphType
    {
        public DogInputType()
        {
            Name = "DogInputType";
            Field<NonNullGraphType<StringGraphType>>("name");
        }
    }
}