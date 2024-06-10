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
    public interface IDiscussions_TopicsManagementService
    {
        Task<GetDiscussions_TopicsResponse> GetDiscussions_Topics();
        Task<GetDiscussions_TopicsByDiscussionIdResponse> GetDiscussions_TopicsByDiscussionId(GetDiscussions_TopicsByDiscussionIdRequest request);
        Task<CreateDiscussions_TopicsResponse> CreateDiscussions_Topics(CreateDiscussions_TopicsRequest request);
        Task<DeleteDiscussions_TopicsResponse> DeleteDiscussions_Topics(DeleteDiscussions_TopicsRequest request);
        Task<DeleteDiscussions_TopicsByIdsResponse> DeleteDiscussions_TopicsByIds(DeleteDiscussions_TopicsByIdsRequest request);
        Task<UpdateDiscussions_TopicsResponse> UpdateDiscussions_Topics(UpdateDiscussions_TopicsRequest request);
    }
}
