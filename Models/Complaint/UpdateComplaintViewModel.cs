using System.ComponentModel.DataAnnotations;

namespace ComplaintRequestSystem.Models.Complaint
{
    public class UpdateComplaintViewModel
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "Complaint text is required")]
        [MinLength(20, ErrorMessage = "Minimum of 20 character required")]
        [MaxLength(150, ErrorMessage = "Maximum of 150 character required")]
        public string ComplaintText { get; set; }

        public string ImageUrl { get; set; }
    }
}
