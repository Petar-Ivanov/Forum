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
    public interface IDiscussionManagementService
    {
        Task<GetDiscussionsResponse> GetDiscussions();
        Task<GetDiscussionByIdResponse> GetDiscussionById(GetDiscussionByIdRequest request);
        Task<GetDiscussionsByTitleResponse> GetDiscussionsByTitle(GetDiscussionsByTitleRequest request);
        Task<GetDiscussionsByTopicResponse> GetDiscussionsByTopic(GetDiscussionsByTopicRequest request);
        Task<CreateDiscussionResponse> CreateDiscussion(CreateDiscussionRequest request);
        Task<DeleteDiscussionResponse> DeleteDiscussion(DeleteDiscussionRequest request);
        Task<UpdateDiscussionResponse> UpdateDiscussion(UpdateDiscussionRequest request);
    }
}
