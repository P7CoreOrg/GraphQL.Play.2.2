using B2BPublisher.Contracts.Models;
using System;
using System.Threading.Tasks;

namespace B2BPublisher.Contracts
{
    public interface IB2BPlublisherStore
    {
        Task<PublishStateResultModel> PublishStateAsync(
            AuthContext authContext, 
            RequestedFields requestedFields,
            PublishStateModel state);
    }
}
