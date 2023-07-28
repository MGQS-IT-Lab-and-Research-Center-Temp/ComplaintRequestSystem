using ComplaintRequestSystem.Models.Request;
using ComplaintRequestSystem.Models;
using ComplaintRequestSystem.Models.Complaint;

namespace ComplaintRequestSystem.Service.Interfaces
{
    public interface IRequestService
    {
        Task<BaseResponseModel> CreateRequest(CreateRequestViewModel request);
        Task<BaseResponseModel> DeleteRequest(string requestId);
        Task<BaseResponseModel> UpdateRequest(string requestId, UpdateRequestViewModel request);
        Task<RequestResponseModel> GetRequest(string requestId);
		Task<RequestsResponseModel> GetRequestsByDepartmentId(string departmentId);
		 Task<RequestsResponseModel> GetAllRequest();
    }
}
