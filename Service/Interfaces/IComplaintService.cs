using ComplaintRequestSystem.Models.Complaint;
using ComplaintRequestSystem.Models;

namespace ComplaintRequestSystem.Service.Interfaces
{
    public interface IComplaintService
    {
        Task<BaseResponseModel> CreateComplaint(CreateComplaintViewModel request);
        Task<BaseResponseModel> DeleteComplaint(string complaintId);
        Task<BaseResponseModel> UpdateComplaint(string complaintId, UpdateComplaintViewModel request);
        Task<ComplaintResponseModel> GetComplaint(string complaintId);
        Task<ComplaintsResponseModel> GetAllComplaint();
        Task<ComplaintsResponseModel> GetComplaintsByDepartmentId(string departmentId);
        Task<ComplaintsResponseModel> DisplayComplaint();
    }
}
