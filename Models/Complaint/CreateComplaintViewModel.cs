using ComplaintRequestSystem.Entities;
using System.ComponentModel.DataAnnotations;

namespace ComplaintRequestSystem.Models.Complaint
{
    public class CreateComplaintViewModel
    {
        [Required(ErrorMessage = "One or more Department need to be selected")]
        public List<string> DepartmentIds { get; set; }

        [Required(ErrorMessage = "Complaint text required")]
        [MinLength(20, ErrorMessage = "Minimum of 20 character required")]
        [MaxLength(150, ErrorMessage = "Maximum of 150 character required")]
        public string ComplaintText { get; set; }
        public string? ImageUrl { get; set; }
    }
}
