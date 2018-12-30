using System.Collections.Generic;
using GraphQL.Types;

namespace P7.GraphQLCore
{
    public interface IMutationFieldRegistration
    {
        void AddGraphTypeFields(MutationCore mutationCore);
    }
    
}