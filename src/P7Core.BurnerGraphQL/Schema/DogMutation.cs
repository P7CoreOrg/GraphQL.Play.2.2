using GraphQL.Types;
using P7Core.Burner;
using P7Core.BurnerGraphQL.Models;
using P7Core.GraphQLCore;

namespace P7Core.BurnerGraphQL.Schema
{
    public class DogMutation : IMutationFieldRegistration
    {
        private IDogStore _dogStore;

        public DogMutation(IDogStore dogStore)
        {
            _dogStore = dogStore;
        }
        public void AddGraphTypeFields(MutationCore mutationCore)
        {
            mutationCore.FieldAsync<DogType>(name: "mutateDog",
                description: null,
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<DogInputType>> { Name = "dog" }),
                resolve: async context =>
                {

                    var dogInput = context.GetArgument<DogInput>("dog");

                    _dogStore.Name = dogInput.Name;
                    return new Dog()
                    {
                        Name = _dogStore.Name
                    };
                }
            );
        }
    }
}