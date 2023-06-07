using ComplaintRequestSystem.Entities;
using System.Linq.Expressions;

namespace ComplaintRequestSystem.Repository.Interfaces
{
    public interface IComplaintRepository : IRepository<Complaint>
    {
        List<Complaint> GetComplaints();
        List<Complaint> GetComplaints(Expression<Func<Complaint, bool>> expression);
        Complaint GetComplaint(Expression<Func<Complaint, bool>> expression);
        List<DepartmentComplaint> GetComplaintByDepartmentId(string id);
        List<DepartmentComplaint> SelectComplaintByDepartment();
    }
}
