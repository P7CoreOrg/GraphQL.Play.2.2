using System;

namespace B2BPublisher.Models
{
    public enum PublishStatus
    {
        Rejected,
        Accepted
    }
    public class PublishStateResultModel
    {
        public PublishStatus status { get; set; }
        public string client_id { get; set; }
        public string client_namespace { get; set; }
    }
}