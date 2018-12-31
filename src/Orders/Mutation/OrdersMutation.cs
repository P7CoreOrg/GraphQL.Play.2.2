using System;
using GraphQL.Types;
using Orders.Models;
using Orders.Schema;
using Orders.Services;
using P7Core.GraphQLCore;

namespace Orders.Mutation
{
    public class OrdersMutation : IMutationFieldRegistration
    {
        private IOrderService _orders;

        public OrdersMutation(IOrderService orders)
        {
            _orders = orders;
        }
        public void AddGraphTypeFields(MutationCore mutationCore)
        {
            mutationCore.FieldAsync<OrderType>(name: "createOrder",
                description: null,
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<OrderCreateInputType>> { Name = "order" }),
                resolve: async context =>
                {
                
                        var orderInput = context.GetArgument<OrderCreateInput>("order");

                        var id = Guid.NewGuid().ToString();
                        var order = new Order(orderInput.Name, orderInput.Description, orderInput.Created, orderInput.CustomerId, id);
                        return _orders.CreateAsync(order);
                } 
            );
            mutationCore.FieldAsync<OrderType>(
                "startOrder",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "orderId" }),
                resolve: async context =>
                {
                    var orderId = context.GetArgument<string>("orderId");
                    return await context.TryAsyncResolve(
                        async c => await _orders.StartAsync(orderId));
                }
            );

            mutationCore.FieldAsync<OrderType>(
                "completeOrder",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "orderId" }),
                resolve: async context =>
                {
                    var orderId = context.GetArgument<string>("orderId");
                    return await context.TryAsyncResolve(
                        async c => await _orders.CompleteAsync(orderId));
                }
            );

            mutationCore.FieldAsync<OrderType>(
                "cancelOrder",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "orderId" }),
                resolve: async context =>
                {
                    var orderId = context.GetArgument<string>("orderId");
                    return await context.TryAsyncResolve(
                        async c => await _orders.CancelAsync(orderId));
                }
            );

            mutationCore.FieldAsync<OrderType>(
                "closeOrder",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "orderId" }),
                resolve: async context =>
                {
                    var orderId = context.GetArgument<string>("orderId");
                    return await context.TryAsyncResolve(
                        async c => await _orders.CloseAsync(orderId));
                }
            );
        }
    }
}