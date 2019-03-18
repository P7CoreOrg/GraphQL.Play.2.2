using GraphQL.Types;
using P7Core.Burner;

namespace P7Core.BurnerGraphQL.Schema
{
    public class DogType : ObjectGraphType<Dog>
    {
        public DogType()
        {
            Field(o => o.Name);
        }
    }
}