using Microsoft.AspNetCore.Mvc.Rendering;

namespace ASPSTART.Areas.Admin.Models.Users
{
    public class UserEditViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public IFormFile? ImageFile { get; set; } = null!;
        public string? NewPassword { get; set; } = string.Empty;

        public List<string> SelectedRoles { get; set; } = new();
        public List<SelectListItem> AvailableRoles { get; set; } = new();
    }
}
