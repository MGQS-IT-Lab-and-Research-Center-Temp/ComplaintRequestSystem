using ComplaintRequestSystem.Entities;

namespace ComplaintRequestSystem.Models.Complaint
{
    public class ComplaintViewModel
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public DepartmentComplaint Department { get; set; }
        public string ComplaintText { get; set; }
    }
}
