using System.ComponentModel.DataAnnotations;

namespace ASPSTART.Models.Users
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? LastName { get; set; } = string.Empty;
        public string Avatar { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}