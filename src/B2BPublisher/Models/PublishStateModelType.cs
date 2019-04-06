using B2BPublisher.Contracts.Models;
using GraphQL.Types;


namespace B2BPublisher.Models
{
    public class PublishStateModelType : ObjectGraphType<PublishStateModel>
    {
        public PublishStateModelType()
        {
            Name = "publishStateModel";
            Field<StringGraphType>("key", "key");
            Field<StringGraphType>("category", "Category");
            Field<StringGraphType>("version", "Version");
            Field<StringGraphType>("state", "State");
        }
    }

}

 