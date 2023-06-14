using ComplaintRequestSystem.Entities;
using System.Linq.Expressions;

namespace ComplaintRequestSystem.Repository.Interfaces
{
    public interface IRequestRepository : IRepository<Request>
    {
        List<Request> GetRequests();
        List<Request> GetRequests(Expression<Func<Request, bool>> expression);
        Request GetRequest(Expression<Func<Request, bool>> expression);
        List<DepartmentRequest> GetRequestsByDepartmentId(string id);
        List<DepartmentRequest> SelectRequestByDepartment();
    }
}
