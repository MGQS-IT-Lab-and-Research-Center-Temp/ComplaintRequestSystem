namespace ComplaintRequestSystem.Models.User
{
    public class UserResponseModel : BaseResponseModel
    {
        public UserViewModel Data { get; set; }
    }

    public class  UsersResponseModel : BaseResponseModel
    {
        public List<UserViewModel> Users { get; set;}
    }
}
