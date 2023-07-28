using ComplaintRequestSystem.Context;
using ComplaintRequestSystem.Repository.Interfaces;

namespace ComplaintRequestSystem.Repository.Implementations
{
    public class UnitOfWork : IUnitOfWork   
    {
        private readonly ComplaintRequestSystemContext _context;
        private bool _disposed = false;
        public IRoleRepository Roles { get; }
        public IUserRepository Users { get; }
        public IComplaintRepository Complaints { get; }
        public IRequestRepository Requests { get; }
        public IDepartmentRepository Departments { get; }

        public UnitOfWork(
            ComplaintRequestSystemContext context,
            IRoleRepository roleRepository,
            IUserRepository userRepository,
            IComplaintRepository complaintRepository,
            IRequestRepository requestRepository,
            IDepartmentRepository departmentRepository)
        {
            _context = context;
            Roles = roleRepository;
            Users = userRepository;
            Complaints = complaintRepository;
            Requests = requestRepository;
            Departments = departmentRepository;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}

