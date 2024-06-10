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
    public interface IDiscussionVoteManagementService
    {
        Task<GetDiscussionVotesResponse> GetVotes();
        Task<GetDiscussionVoteByIdsResponse> GetVoteByIds(GetDiscussionVoteByIdsRequest request);
        Task<CreateVoteResponse> CreateVote(CreateVoteRequest request);
        Task<DeleteVoteResponse> DeleteVote(DeleteVoteRequest request);
        Task<UpdateDiscussionVoteResponse> UpdateVote(UpdateVoteRequest request);
    }
}
