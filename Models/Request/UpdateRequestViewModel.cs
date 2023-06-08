using System.ComponentModel.DataAnnotations;

namespace ComplaintRequestSystem.Models.Request
{
    public class UpdateRequestViewModel
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "Request text cannot be empty")]
        [MinLength(20, ErrorMessage = "The minimum length is 20.")]
        public string RequestText { get; set; }
    }
}
