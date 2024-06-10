using Messaging.Requests.Create;
using Messaging.Requests.Delete;
using Messaging.Requests.Get;
using Messaging.Requests.Update;
using Messaging.Responses.Create;
using Messaging.Responses.Delete;
using Messaging.Responses.Get;
using Messaging.Responses.GetBy;
using Messaging.Responses.Update;

namespace ApplicationServices.Interfaces
{
    public interface ITopicManagementService
    {
        Task<GetTopicsResponse> GetTopics();
        Task<GetTopicByIdResponse> GetTopicById(GetTopicByIdRequest request);
        Task<GetTopicsByNameResponse> GetTopicsByName(GetTopicsByNameRequest request);
        Task<CreateTopicResponse> CreateTopic(CreateTopicRequest request);
        Task<DeleteTopicResponse> DeleteTopic(DeleteTopicRequest request);
        Task<UpdateTopicResponse> UpdateTopic(UpdateTopicRequest request);
    }
}
