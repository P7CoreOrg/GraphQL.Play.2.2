using B2BPublisher.Models;
using GraphQL.Types;
using P7Core.GraphQLCore;
using System;
using System.Linq;
using System.Security.Claims;

namespace B2BPublisher.Mutation
{
    public class PublishStateMutation : IMutationFieldRegistration
    {
        (string clientId, string clientNamespace) GetClientInfoFromPincipal(ClaimsPrincipal principal)
        {
            var query = from item in principal.Claims
                        where item.Type == "client_id"
                        select item.Value;
            var clientId = query.FirstOrDefault();

            query = from item in principal.Claims
                        where item.Type == "client_namespace"
                        select item.Value;
            var clientNamespace = query.FirstOrDefault();


            return (clientId,clientNamespace);

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


                       var customer = context.GetArgument<PublishStateModel>("input");
                       return new PublishStateResultModel()
                       {
                           status = PublishStatus.Accepted,
                           client_id = clientInfo.clientId,
                           client_namespace = clientInfo.clientNamespace
                       };

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
