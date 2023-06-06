namespace ComplaintRequestSystem.Entities
{
    public class Complaint : BaseEntity
    {
        public string UserId { get; set; }
        public User User { get; set; }
        public Department Department { get; set; }
        public bool IsClosed { get; set; }
        public ICollection<DepartmentComplaint> DepartmentComplaint { get; set; } = new HashSet<DepartmentComplaint>();
        public string ComplaintText { get; set; }
    }
}
