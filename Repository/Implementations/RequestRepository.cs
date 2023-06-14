using ComplaintRequestSystem.Context;
using ComplaintRequestSystem.Entities;
using ComplaintRequestSystem.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ComplaintRequestSystem.Repository.Implementations
{
    public class RequestRepository : BaseRepository<Request>, IRequestRepository
    {
        public RequestRepository(ComplaintRequestSystemContext context) : base(context)
        {
        }

        public Request GetRequest(Expression<Func<Request, bool>> expression)
        {
            var request = _context.Requests
                .Include(c => c.User)
                .SingleOrDefault(expression);

            return request;
        }

        public List<Request> GetRequests()
        {
            var requests = _context.Requests
                .Include(uq => uq.User)
                .Include(d => d.Department)
                .ToList();

            return requests;
        }

        public List<Request> GetRequests(Expression<Func<Request, bool>> expression)
        {
            var requests = _context.Requests
                .Where(expression)
                .Include(u => u.User)
                .Include(d => d.Department)
                .ToList();
            return requests;
        }

        public List<DepartmentRequest> GetRequestsByDepartmentId(string departmentId)
        {
            var requests = _context.DepartmentRequests
                .Include(d => d.Department)
                .Include(r => r.Request)
                .ThenInclude(c => c.User)
                .Where(c => c.DepartmentId.Equals(departmentId))
            .ToList();

            return requests;
        }

        public List<DepartmentRequest> SelectRequestByDepartment()
        {
            var requests = _context.DepartmentRequests
                .Include(d => d.Department)
                .Include(r => r.Request)
                .ThenInclude(u => u.User)
                .ToList();

            return requests;
        }
    }
}

