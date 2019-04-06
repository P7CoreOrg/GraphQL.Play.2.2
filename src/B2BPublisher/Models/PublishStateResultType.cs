using B2BPublisher.Contracts.Models;
using B2BPublisher.Models;
using GraphQL.Types;


namespace B2BPublisher.Models
{
    public class PublishStatusEnumType : EnumerationGraphType<PublishStatus>
    {
    }
    public class PublishStateResultType : ObjectGraphType<PublishStateResultModel>
    {
        public PublishStateResultType()
        {
            Name = "publishStateResult";
            Field<PublishStatusEnumType>("status", "publish status");
            Field<StringGraphType>("client_id", "the client_id that made the call");
            Field<StringGraphType>("client_namespace", "the client_namespace that made the call");
        }
    }

}