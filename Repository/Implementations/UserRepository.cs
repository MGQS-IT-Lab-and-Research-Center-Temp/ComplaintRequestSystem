using ComplaintRequestSystem.Context;
using ComplaintRequestSystem.Entities;
using ComplaintRequestSystem.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ComplaintRequestSystem.Repository.Implementations
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(ComplaintRequestSystemContext context) : base(context)
        {
        }

        public User GetUser(Expression<Func<User, bool>> expression)
        {
            return _context.Users
                .Include(r => r.Role)
                .SingleOrDefault(expression);
        }
    }
}
