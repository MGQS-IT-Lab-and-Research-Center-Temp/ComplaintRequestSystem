using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ComplaintRequestSystem.Models.Auth
{
    public class SignUpViewModel
    {
        [Required(ErrorMessage = "Username is required.")]
        [Range(3, 10, ErrorMessage = "The minimum length is 3 and the maximum length is 10!")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("Password", ErrorMessage =
            "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
