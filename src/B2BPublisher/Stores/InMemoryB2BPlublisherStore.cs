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
        Dictionary<(string clientId, string clientNamespace), PublishStateModel> Database {
            get
            {
                if(_database == null)
                {
                    _database = new Dictionary<(string clientId, string clientNamespace), PublishStateModel>();
                }
                return _database;
            }
        }

        public async Task<PublishStateModel> GetPublishStateAsync(AuthContext authContext, RequestedFields requestedFields)
        {
            if (Database.ContainsKey((authContext.ClientId, authContext.ClientNamespace))){
                return Database[(authContext.ClientId, authContext.ClientNamespace)];
            }
            return null;
           
        }

        public async Task<PublishStateResultModel> PublishStateAsync(AuthContext authContext, RequestedFields requestedFields, PublishStateModel payload)
        {
            var clientIdRequested = requestedFields.Fields.Contains("client_id");
            var clientNamespaceRequested = requestedFields.Fields.Contains("client_namespace");
            Database[(authContext.ClientId, authContext.ClientNamespace)] = payload;
            return new PublishStateResultModel()
            {
                status = PublishStatus.Accepted,
                client_id = clientIdRequested ? authContext.ClientId : null,
                client_namespace = clientNamespaceRequested ? authContext.ClientNamespace : null
            };
        }
    }
}
