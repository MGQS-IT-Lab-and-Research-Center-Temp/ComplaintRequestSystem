using System.ComponentModel.DataAnnotations;

namespace ComplaintRequestSystem.Models.Request
{
    public class UpdateRequestViewModel
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "Request text is required")]
        [MinLength(20, ErrorMessage = "Minimum of 20 character required")]
        [MaxLength(150, ErrorMessage = "Maximum of 150 character required")]
        public string RequestText { get; set; }

        public string ImageUrl { get; set; }
    }
}
