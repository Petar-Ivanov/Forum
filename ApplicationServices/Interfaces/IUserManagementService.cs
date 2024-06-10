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
    public interface IUserManagementService
    {
        Task<GetUsersResponse> GetUsers();
        Task<GetUserByUsernamePasswordResponse> GetUserByUsernamePassword(GetUserByUsernamePasswordRequest request);
        Task<GetUsersByUsernameResponse> GetUsersByUsername(GetUsersByUsernameRequest request);
        Task<GetUserByIdResponse> GetUserById(GetUserByIdRequest request);
        Task<CreateUserResponse> CreateUser(CreateUserRequest request);
        Task<DeleteUserResponse> DeleteUser(DeleteUserRequest request);
        Task<UpdateUserResponse> UpdateUser(UpdateUserRequest request);
    }
}
