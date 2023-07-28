using ComplaintRequestSystem.Entities;
using System.Linq.Expressions;

namespace ComplaintRequestSystem.Repository.Interfaces
{
    public interface IComplaintRepository : IRepository<Complaint>
    {
        Task<List<Complaint>> GetComplaints();
        Task<List<Complaint>> GetComplaints(Expression<Func<Complaint, bool>> expression);
        Task<Complaint> GetComplaint(Expression<Func<Complaint, bool>> expression);
        Task<List<DepartmentComplaint>> GetComplaintByDepartmentId(string id);
        Task<List<DepartmentComplaint>> SelectComplaintByDepartment();
    }
}
