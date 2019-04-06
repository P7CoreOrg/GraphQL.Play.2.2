using System;
using System.Collections.Generic;

namespace B2BPublisher.Contracts.Models
{
    public class AuthContext
    {
        public string ClientId { get; set; }
        public string ClientNamespace { get; set; }
        public List<string> Scopes { get; set; }
    }

    public class RequestedFields
    {
        public List<string> Fields { get; set; }
    }
    public class PublishStateModel
    {

        public string Key { get; set; }
        public string Category { get; set; }
        public string Version { get; set; }
        public string State { get; set; }

    }
}
