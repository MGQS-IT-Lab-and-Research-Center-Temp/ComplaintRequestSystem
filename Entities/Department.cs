namespace ComplaintRequestSystem.Entities
{
    public class Department : BaseEntity
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public ICollection<DepartmentComplaint> DepartmentComplaints { get; set; } = new HashSet<DepartmentComplaint>();
        public ICollection<DepartmentRequest> DepartmentRequest { get; set; } = new HashSet<DepartmentRequest>();
    }
}
