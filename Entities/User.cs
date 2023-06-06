using System.Data;

namespace ComplaintRequestSystem.Entities
{
    public class User : BaseEntity
    {
        public string UserName { get; set; }
        public string HashSalt { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public string RoleId { get; set; }
        public Role Role { get; set; }
        public ICollection<Complaint> Complaints { get; set; } = new HashSet<Complaint>();
        public ICollection<Request> Requests { get; set; } = new HashSet<Request>();
    }
}
