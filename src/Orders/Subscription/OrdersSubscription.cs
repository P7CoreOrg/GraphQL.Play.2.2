using GraphQL.Types;
using GraphQL.Subscription;
using GraphQL.Resolvers;
using Orders.Services;
using Orders.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Reactive.Linq;
using P7.GraphQLCore;
using Orders.Schema;

namespace Orders.Subscription
{
    public class OrdersSubscription : ISubscriptionFieldRegistration
    {
        private readonly IOrderEventService _events;

        public OrdersSubscription(IOrderEventService events)
        {
            _events = events;
        }

        public void AddGraphTypeFields(SubscriptionCore subscriptionCore)
        {
            var orderEventType = new EventStreamFieldType
            {
                Name = "orderEvent",
                Arguments = new QueryArguments(new QueryArgument<ListGraphType<OrderStatusesEnum>>
                {
                    Name = "statuses"
                }),
                Type = typeof(OrderEventType),
                Resolver = new FuncFieldResolver<OrderEvent>(ResolveEvent),
                Subscriber = new EventStreamResolver<OrderEvent>(Subscribe)
            };
            subscriptionCore.AddField(orderEventType);
        }

        private OrderEvent ResolveEvent(ResolveFieldContext context)
        {
            var orderEvent = context.Source as OrderEvent;
            return orderEvent;
        }

        private IObservable<OrderEvent> Subscribe(ResolveEventStreamContext context)
        {
            var statusList = context.GetArgument<IList<OrderStatuses>>("statuses", new List<OrderStatuses>());

            if (statusList.Count > 0)
            {
                OrderStatuses statuses = 0;

                foreach (var status in statusList)
                {
                    statuses = statuses | status;
                }
                return _events.EventStream().Where(e => (e.Status & statuses) == e.Status);
            }
            else
            {
                return _events.EventStream();
            }

        }
    }
}
