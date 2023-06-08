using ComplaintRequestSystem.Entities;

namespace ComplaintRequestSystem.Models.Request
{
    public class RequestViewModel
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public DepartmentRequest Department { get; set; }
        public string RequestText { get; set; }
    }
}
