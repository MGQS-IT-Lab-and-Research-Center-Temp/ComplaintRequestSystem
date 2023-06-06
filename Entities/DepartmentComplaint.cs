namespace ComplaintRequestSystem.Entities
{
    public class DepartmentComplaint : BaseEntity
    {
        public string DepartmentId { get; set; }
        public Department Department { get; set; }
        public string ComplaintId { get; set; }
        public Complaint Complaint { get; set; }
    }
}
