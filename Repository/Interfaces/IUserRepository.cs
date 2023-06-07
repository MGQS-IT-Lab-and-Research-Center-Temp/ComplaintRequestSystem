using ComplaintRequestSystem.Entities;
using System.Linq.Expressions;

namespace ComplaintRequestSystem.Repository.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        User GetUser(Expression<Func<User, bool>> expression);
    }
}
