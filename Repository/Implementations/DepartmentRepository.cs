using ComplaintRequestSystem.Context;
using ComplaintRequestSystem.Entities;
using ComplaintRequestSystem.Repository.Interfaces;

namespace ComplaintRequestSystem.Repository.Implementations
{
    public class DepartmentRepository : BaseRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(ComplaintRequestSystemContext context) : base(context)
        {
        }
    }
}
