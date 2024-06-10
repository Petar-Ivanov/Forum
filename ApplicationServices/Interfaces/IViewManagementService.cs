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
    public interface IViewManagementService
    {
        Task<GetViewsResponse> GetViews();
        Task<GetViewByIdsResponse> GetViewByIds(GetViewByIdsRequest request);
        Task<CreateViewResponse> CreateView(CreateViewRequest request);
        Task<DeleteViewResponse> DeleteView(DeleteViewRequest request);
        Task<UpdateViewResponse> UpdateView(UpdateViewRequest request);
    }
}
