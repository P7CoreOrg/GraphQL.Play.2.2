using B2BPublisher.Contracts;
using B2BPublisher.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace B2BPublisher.Stores
{
    class InMemoryB2BPlublisherStore : IB2BPlublisherStore
    {
        public async Task<PublishStateResultModel> PublishStateAsync(AuthContext authContext, RequestedFields requestedFields, PublishStateModel state)
        {
            var clientIdRequested = requestedFields.Fields.Contains("client_id");
            var clientNamespaceRequested = requestedFields.Fields.Contains("client_namespace");
            return new PublishStateResultModel()
            {
                status = PublishStatus.Accepted,
                client_id = clientIdRequested ? authContext.ClientId : null,
                client_namespace = clientNamespaceRequested ? authContext.ClientNamespace : null
            };
        }
    }
}
