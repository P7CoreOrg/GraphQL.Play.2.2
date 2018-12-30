using GraphQL.Types;

namespace P7.GraphQLCore
{
    public class MutationCore : ObjectGraphType<object>
    {
        private int _count;

        public MutationCore(IMutationFieldRecordRegistrationStore fieldStore)
        {
            Name = "mutation";
            fieldStore.AddGraphTypeFields(this);
            _count = fieldStore.Count;
        }

        public int RegistrationCount => _count;
    }
}