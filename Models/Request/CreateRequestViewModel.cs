using ComplaintRequestSystem.Entities;
using System.ComponentModel.DataAnnotations;

namespace ComplaintRequestSystem.Models.Request
{
    public class CreateRequestViewModel
    {
        public string UserId { get; set; }
        public List<string> RequestsIds { get; set; }
        public DepartmentRequest Department { get; set; }
        [Required(ErrorMessage = "Request text cannot be empty")]
        [MinLength(20, ErrorMessage = "The minimum length is 20.")]
        public string RequestText { get; set; }
    }
}
