using System;
using GraphQL;
using GraphQL.Types;
using P7.GraphQLCore;

namespace P7.GraphQLCore.Mutation
{
    public class PlaceHolderMutation : IMutationFieldRecordRegistration
    {
        public void AddGraphTypeFields(MutationCore mutationCore)
        {
            mutationCore.FieldAsync<BooleanGraphType>(name: "_placeHolder",
                description: "This is here so we have at least one mutation",
                resolve: async context => true,
                deprecationReason: null
            );
        }
    }
}