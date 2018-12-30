using System;
using GraphQL;
using GraphQL.Types;

namespace P7.GraphQLCore
{
    public class SchemaCore : Schema
    {
        public SchemaCore(QueryCore query,
            MutationCore mutation,
            SubscriptionCore subscription, 
            IDependencyResolver resolver)
        {
            Query = query;
            Mutation = mutation;
            Subscription = subscription;
            DependencyResolver = resolver;
        }
    }
}