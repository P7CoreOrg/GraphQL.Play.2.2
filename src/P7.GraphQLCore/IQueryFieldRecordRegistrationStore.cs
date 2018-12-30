namespace P7.GraphQLCore
{
    public interface IQueryFieldRecordRegistrationStore
    {
        int Count { get; }
        void AddGraphTypeFields(QueryCore queryCore);
    }
}