using System.ComponentModel.DataAnnotations;

namespace ComplaintRequestSystem.Models.Complaint
{
    public class UpdateComplaintViewModel
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "Complaint text cannot be empty")]
        [MinLength(20, ErrorMessage = "The minimum length is 20.")]
        public string ComplaintText { get; set; }
    }
}
