using ComplaintRequestSystem.Context;
using ComplaintRequestSystem.Entities;
using ComplaintRequestSystem.Repository.Interfaces;

namespace ComplaintRequestSystem.Repository.Implementations
{
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        public RoleRepository(ComplaintRequestSystemContext context) : base(context)
        {
        }
    }
}
