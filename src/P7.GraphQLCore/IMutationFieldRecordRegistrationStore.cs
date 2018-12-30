using System.Collections.Generic;
using System.Linq;
using GraphQL.Types;

namespace P7.GraphQLCore
{
    public interface IMutationFieldRecordRegistrationStore
    {
        int Count { get; }
        void AddGraphTypeFields(MutationCore mutationCore);
    }
}

