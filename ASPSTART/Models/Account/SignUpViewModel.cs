
using System.ComponentModel.DataAnnotations;

namespace ASPSTART.Models.Account
{
    public class SignUpViewModel
    {
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; } = string.Empty;
        [Display(Name = "Password")]
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = string.Empty;
        [Display(Name = "FirstName")]
        public string? FirstName { get; set; } = string.Empty;
        [Display(Name = "LastName")]
        public string? LastName { get; set; } = string.Empty;
        [Display(Name = "Avatar")]
        public IFormFile Image { get; set; } = null;
    }
}
