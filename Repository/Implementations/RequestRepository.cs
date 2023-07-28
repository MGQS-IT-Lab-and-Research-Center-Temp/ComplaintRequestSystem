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

        public async Task<Request> GetRequest(Expression<Func<Request, bool>> expression)
        {
            var request = await _context.Requests
                .Include(c => c.User)
                .SingleOrDefaultAsync(expression);

            return request;
        }

        public async Task<List<Request>> GetRequests()
        {
            var requests = await _context.Requests
                .Include(uq => uq.User)
                .ToListAsync();

            return requests;
        }

        public async Task<List<Request>> GetRequests(Expression<Func<Request, bool>> expression)
        {
            var requests = await _context.Requests
                .Where(expression)
                .Include(u => u.User)
                .ToListAsync();
            return requests;
        }

        public async Task<List<DepartmentRequest>> GetRequestsByDepartmentId(string departmentId)
        {
            var requests = await _context.DepartmentRequests
                .Include(d => d.Department)
                .Include(r => r.Request)
                .ThenInclude(c => c.User)
                .Where(c => c.DepartmentId.Equals(departmentId))
            .ToListAsync();

            return requests;
        }

        public async Task<List<DepartmentRequest>> SelectRequestByDepartment()
        {
            var requests = await _context.DepartmentRequests
                .Include(d => d.Department)
                .Include(r => r.Request)
                .ThenInclude(u => u.User)
                .ToListAsync();

            return requests;
        }
    }
}

