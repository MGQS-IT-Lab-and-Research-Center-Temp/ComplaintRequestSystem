using ComplaintRequestSystem.Models.Auth;
using ComplaintRequestSystem.Models.User;
using ComplaintRequestSystem.Models;

namespace ComplaintRequestSystem.Service.Interfaces
{
    public interface IUserService
    {
        UserResponseModel GetUser(string userId);
        BaseResponseModel Register(SignUpViewModel request, string roleName = null);
        UserResponseModel Login(LoginViewModel request);
    }
}
