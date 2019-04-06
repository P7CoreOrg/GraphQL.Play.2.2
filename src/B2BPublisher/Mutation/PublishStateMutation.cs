using B2BPublisher.Contracts;
using B2BPublisher.Contracts.Models;
using B2BPublisher.Models;
using GraphQL.Types;
using P7Core.GraphQLCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace B2BPublisher.Mutation
{
    public class PublishStateMutation : IMutationFieldRegistration
    {
        private IB2BPlublisherStore _b2BPlublisherStore;

        public PublishStateMutation(IB2BPlublisherStore b2BPlublisherStore)
        {
            _b2BPlublisherStore = b2BPlublisherStore;
        }
        (string clientId, string clientNamespace, List<string> scopes) GetClientInfoFromPincipal(ClaimsPrincipal principal)
        {
            var query = from item in principal.Claims
                        where item.Type == "client_id"
                        select item.Value;
            var clientId = query.FirstOrDefault();

            query = from item in principal.Claims
                    where item.Type == "client_namespace"
                    select item.Value;
            var clientNamespace = query.FirstOrDefault();

            query = from item in principal.Claims
                    where item.Type == "scope"
                    select item.Value;
            var scopes = query.ToList();
            return (clientId, clientNamespace, scopes);

        }

        public void AddGraphTypeFields(MutationCore mutationCore)
        {
            mutationCore.FieldAsync<PublishStateResultType>(name: "publishState",
               description: null,
               arguments: new QueryArguments(new QueryArgument<PublishStateInputType> { Name = "input" }),
               resolve: async context =>
               {
                   try
                   {
                       // Authorization worked, but who is calling us
                       var graphQLUserContext = context.UserContext as GraphQLUserContext;
                       var principal = graphQLUserContext.HttpContextAccessor.HttpContext.User;

                       // Since this is a B2B api, there probably will not be a subject, hence no real user
                       // What we have here is an organization, so we have to find out the clientId, and the client_namespace

                       var clientInfo = GetClientInfoFromPincipal(principal);

                       bool clientIdRequested = context.SubFields.ContainsKey("client_id");
                       bool clientNamespaceRequested = context.SubFields.ContainsKey("client_namespace");
                       var requestedFields = (from item in context.SubFields
                                              select item.Key).ToList();

                       var input = context.GetArgument<PublishStateModel>("input");

                       var result = await _b2BPlublisherStore.PublishStateAsync(
                           new Contracts.Models.AuthContext
                           {
                               ClientId = clientInfo.clientId,
                               ClientNamespace = clientInfo.clientNamespace,
                               Scopes = clientInfo.scopes

                           }, new Contracts.Models.RequestedFields
                           {
                               Fields = requestedFields
                           }, 
                           input);


                       return result;

                   }
                   catch (Exception e)
                   {

                   }
                   return false;
                   //                    return await Task.Run(() => { return ""; });

               },
               deprecationReason: null
           );
        }
    }
}
