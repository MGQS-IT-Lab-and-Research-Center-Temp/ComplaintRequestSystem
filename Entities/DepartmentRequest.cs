namespace ComplaintRequestSystem.Entities
{
    public class DepartmentRequest : BaseEntity
    {
        public string DepartmentId { get; set; }
        public Department Department { get; set; }
        public string RequestId { get; set; }
        public Request Request { get; set; }
    }
}
