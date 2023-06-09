﻿using System.ComponentModel.DataAnnotations;

namespace ComplaintRequestSystem.Models.Auth
{
	public class SignUpViewModel
    {
        [Required(ErrorMessage = "Username is required.")]
		[MinLength(3, ErrorMessage = "The minimum length is 3.")]
		[MaxLength(10, ErrorMessage = "The maximum length is 10.")]
		public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required.")]
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
