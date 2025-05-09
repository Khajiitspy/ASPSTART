using System.ComponentModel.DataAnnotations;

namespace ASPSTART.Models.Account
{
    public class ResetPasswordViewModel
    {
        public string Email { get; set; } = string.Empty;

        [Display(Name = "New Password")]
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage = "Password confirmation is required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;

        public string Token { get; set; } = string.Empty;
    }
}
