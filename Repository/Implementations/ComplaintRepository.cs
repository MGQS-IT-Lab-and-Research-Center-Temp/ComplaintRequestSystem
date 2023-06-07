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

        public Complaint GetComplaint(Expression<Func<Complaint, bool>> expression)
        {
            var complaint = _context.Complaints
                .Include(c => c.User)
            .SingleOrDefault(expression);

            return complaint;
        }

        public List<Complaint> GetComplaints()
        {
            var complaints = _context.Complaints
                .Include(uq => uq.User)
                .Include(d => d.Department)
                .ToList();
            return complaints;
        }

        public List<Complaint> GetComplaints(Expression<Func<Complaint, bool>> expression)
        {
            var complaints = _context.Complaints
                .Where(expression)
                .Include(u => u.User)
                .Include(d => d.Department)
                .ToList();
            return complaints;
        }

        public List<DepartmentComplaint> GetComplaintByDepartmentId(string departmentId)
        {
            var complaints = _context.DepartmentComplaints
                .Include(c => c.Complaint)
                .ThenInclude(c => c.User)
                .Where(c => c.DepartmentId.Equals(departmentId))
            .ToList();
            return complaints;
        }

        public List<DepartmentComplaint> SelectComplaintByDepartment()
        {
            var complaints = _context.DepartmentComplaints
                .Include(d => d.Department)
                .Include(c => c.Complaint)
                .ThenInclude(u => u.User)
                .ToList();

            return complaints;
        }
    }
}

