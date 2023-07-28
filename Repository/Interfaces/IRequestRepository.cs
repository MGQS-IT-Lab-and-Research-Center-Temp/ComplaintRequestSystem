using ComplaintRequestSystem.Entities;
using System.Linq.Expressions;

namespace ComplaintRequestSystem.Repository.Interfaces
{
    public interface IRequestRepository : IRepository<Request>
    {
        Task<List<Request>> GetRequests();
        Task<List<Request>> GetRequests(Expression<Func<Request, bool>> expression);
        Task<Request> GetRequest(Expression<Func<Request, bool>> expression);
        Task<List<DepartmentRequest>> GetRequestsByDepartmentId(string id);
        Task<List<DepartmentRequest>> SelectRequestByDepartment();
    }
}
