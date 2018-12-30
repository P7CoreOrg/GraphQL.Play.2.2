namespace P7.GraphQLCore
{
    public interface IQueryFieldRegistrationStore
    {
        int Count { get; }
        void AddGraphTypeFields(QueryCore queryCore);
    }
}