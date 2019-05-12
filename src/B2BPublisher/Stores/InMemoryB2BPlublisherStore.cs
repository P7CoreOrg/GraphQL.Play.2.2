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
        Dictionary<(string clientId, string clientNamespace), PublishStateModel> _database;
        Dictionary<(string clientId, string clientNamespace), PublishStateModel> Database
        {
            get
            {
                if (_database == null)
                {
                    _database = new Dictionary<(string clientId, string clientNamespace), PublishStateModel>();
                }
                return _database;
            }
        }

        public Task<PublishStateModel> GetPublishStateAsync(AuthContext authContext, RequestedFields requestedFields)
        {
            PublishStateModel result = null;
            if (Database.ContainsKey((authContext.ClientId, authContext.ClientNamespace)))
            {
                result = Database[(authContext.ClientId, authContext.ClientNamespace)];
            }
            return Task.FromResult(result);

        }

        public Task<PublishStateResultModel> PublishStateAsync(
            AuthContext authContext,
            RequestedFields requestedFields,
            PublishStateModel state)
        {
            var clientIdRequested = requestedFields.Fields.Contains("client_id");
            var clientNamespaceRequested = requestedFields.Fields.Contains("client_namespace");
            Database[(authContext.ClientId, authContext.ClientNamespace)] = state;
            var result = new PublishStateResultModel()
            {
                status = PublishStatus.Accepted,
                client_id = clientIdRequested ? authContext.ClientId : null,
                client_namespace = clientNamespaceRequested ? authContext.ClientNamespace : null
            };
            return Task.FromResult(result);
        }
    }
}
