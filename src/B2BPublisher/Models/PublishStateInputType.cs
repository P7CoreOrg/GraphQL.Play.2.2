using GraphQL.Types;
using System;

namespace B2BPublisher.Models
{
    public class PublishStateInputType : InputObjectGraphType
    {
        public PublishStateInputType()
        {
            Name = "publishStateInput";
            Field<NonNullGraphType<StringGraphType>>("key");
            Field<NonNullGraphType<StringGraphType>>("category");
            Field<NonNullGraphType<StringGraphType>>("version");
            Field<NonNullGraphType<StringGraphType>>("state");
        }
    }

}
