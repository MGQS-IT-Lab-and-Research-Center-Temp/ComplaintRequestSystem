using ComplaintRequestSystem.Entities;
using ComplaintRequestSystem.Helper.Enum;

namespace ComplaintRequestSystem.Models.Complaint
{
    public class ComplaintViewModel
    {
		public string Id { get; set; }
		public string UserId { get; set; }
		public string ComplaintText { get; set; }
		public string ImageUrl { get; set; }
		public string UserName { get; set; }
		public ComplaintStatus Status { get; set; }   
        public DepartmentComplaint Department { get; set; }
        
    }
}
