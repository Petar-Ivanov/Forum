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
    public interface ICommentManagementService
    {
        Task<GetCommentsResponse> GetComments();
        Task<GetCommentsByIdResponse> GetCommentsById(GetCommentsByIdRequest request);
        Task<GetCommentByIdResponse> GetCommentById(GetCommentByIdRequest request);
        Task<CreateCommentResponse> CreateComment(CreateCommentRequest request);
        Task<DeleteCommentResponse> DeleteComment(DeleteCommentRequest request);
        Task<UpdateCommentResponse> UpdateComment(UpdateCommentRequest request);
    }
}
