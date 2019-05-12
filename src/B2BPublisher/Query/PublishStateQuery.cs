using B2BPublisher.Contracts;
using B2BPublisher.Extensions;
using B2BPublisher.Models;
using GraphQL;
using P7Core.GraphQLCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace B2BPublisher.Query
{
    public class PublishStateQuery : IQueryFieldRegistration
    {
        private readonly IB2BPlublisherStore _b2bPublisherStore;

        public PublishStateQuery(IB2BPlublisherStore b2BPublisherStore)
        {
            _b2bPublisherStore = b2BPublisherStore;
        }

        public void AddGraphTypeFields(QueryCore queryCore)
        {
            queryCore.FieldAsync<PublishStateModelType>(name: "publishState",
                description: $"Fetches current state.",
                resolve: async context =>
                {
                    // Authorization worked, but who is calling us
                    var graphQLUserContext = context.UserContext as GraphQLUserContext;
                    var principal = graphQLUserContext.HttpContextAccessor.HttpContext.User;

                    // Since this is a B2B api, there probably will not be a subject, hence no real user
                    // What we have here is an organization, so we have to find out the clientId, and the client_namespace

                    var authContext = principal.ToAuthContext();
                    var requestedFields = (from item in context.SubFields
                                           select item.Key).ToList();

                    var result = await _b2bPublisherStore.GetPublishStateAsync(
                        authContext, new Contracts.Models.RequestedFields
                        {
                            Fields = requestedFields
                        });
                    return result;
                },
                deprecationReason: null);
        }
    }
}
