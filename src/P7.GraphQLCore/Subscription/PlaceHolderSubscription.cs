using GraphQL.Types;

namespace P7.GraphQLCore.Subscription
{
    public class PlaceHolderSubscription : ISubscriptionFieldRegistration
    {
        public void AddGraphTypeFields(MutationCore mutationCore)
        {
          
        }

        public void AddGraphTypeFields(SubscriptionCore subscriptionCore)
        {
            subscriptionCore.FieldAsync<BooleanGraphType>(name: "_placeHolder",
              description: "This is here so we have at least one subscription",
              resolve: async context => true,
              deprecationReason: null
          );
        }
    }
}