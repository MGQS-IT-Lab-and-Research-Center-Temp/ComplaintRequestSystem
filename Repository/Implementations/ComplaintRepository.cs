using ComplaintRequestSystem.Context;
using ComplaintRequestSystem.Entities;
using ComplaintRequestSystem.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ComplaintRequestSystem.Repository.Implementations
{
    public class ComplaintRepository : BaseRepository<Complaint>, IComplaintRepository
    {
        public ComplaintRepository(ComplaintRequestSystemContext context) : base(context)
        {
        }

        public async Task<Complaint> GetComplaint(Expression<Func<Complaint, bool>> expression)
        {
            var complaint = await _context.Complaints
                .Include(c => c.User)
            .SingleOrDefaultAsync(expression);

            return complaint;
        }

        public async Task<List<Complaint>> GetComplaints()
        {
			var complaints = await _context.Complaints
                .Include(uq => uq.User)
                .Include(d => d.Department)
                .ToListAsync();

            return complaints;
        }

        public async Task<List<Complaint>> GetComplaints(Expression<Func<Complaint, bool>> expression)
        {
            var complaints = await _context.Complaints
                .Where(expression)
                .Include(u => u.User)
                .Include(d => d.Department)
                .ToListAsync();

            return complaints;
        }

        public async Task<List<DepartmentComplaint>> GetComplaintByDepartmentId(string departmentId)
        {
            var complaints = await _context.DepartmentComplaints
                .Include(d => d.Department)
                .Include(c => c.Complaint)
                .ThenInclude(c => c.User)
                .Where(c => c.DepartmentId.Equals(departmentId))
                .ToListAsync();

            return complaints;
        }

        public async Task<List<DepartmentComplaint>> SelectComplaintByDepartment()
        {
            var complaints = await _context.DepartmentComplaints
                .Include(d => d.Department)
                .Include(c => c.Complaint)
                .ThenInclude(u => u.User)
                .ToListAsync();

            return complaints;
        }
    }
}

