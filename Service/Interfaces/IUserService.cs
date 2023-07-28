using ComplaintRequestSystem.Models.Auth;
using ComplaintRequestSystem.Models.User;
using ComplaintRequestSystem.Models;

namespace ComplaintRequestSystem.Service.Interfaces
{
    public interface IUserService
    {
        Task<UserResponseModel> GetUser(string userId);
        Task<BaseResponseModel> Register(SignUpViewModel request, string roleName = null);
        Task<UserResponseModel> Login(LoginViewModel request);
    }
}
