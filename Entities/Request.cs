using ComplaintRequestSystem.Helper.Enum;

namespace ComplaintRequestSystem.Entities
{
    public class Request : BaseEntity
    {
        public string UserId { get; set; }
        public User User { get; set; }
        public Department Department { get; set; }
        public bool IsClosed { get; set; }
        public RequestStatus status { get; set; }
        public ICollection<DepartmentRequest> DepartmentRequest { get; set; } = new HashSet<DepartmentRequest>();
        public string RequestText { get; set; }
    }
}
