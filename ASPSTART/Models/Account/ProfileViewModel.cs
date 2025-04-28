using System.ComponentModel.DataAnnotations;

namespace ASPSTART.Models.Account
{
    public class ProfileViewModel
    {
        public string? Image { get; set; } = string.Empty;
        [Display(Name = "UserName")]
        public string UserName { get; set; } = string.Empty;
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;
        [Display(Name = "Password")]
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = string.Empty;
        [Display(Name = "FirstName")]
        public string? FirstName { get; set; } = string.Empty;
        [Display(Name = "LastName")]
        public string? LastName { get; set; } = string.Empty;
    }
}
