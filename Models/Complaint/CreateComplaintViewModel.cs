using ComplaintRequestSystem.Entities;
using System.ComponentModel.DataAnnotations;

namespace ComplaintRequestSystem.Models.Complaint
{
    public class CreateComplaintViewModel
    {
        public string UserId { get; set; }
        public List<string> ComplaintsIds { get; set; }
        public DepartmentComplaint Department { get; set; }
        [Required(ErrorMessage = "Complaint text cannot be empty")]
        [MinLength(20, ErrorMessage = "The minimum length is 20.")]
        public string ComplaintText { get; set; }
    }
}
