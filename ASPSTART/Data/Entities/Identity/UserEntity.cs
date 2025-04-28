using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ASPSTART.Data.Entities.Identity
{
    public class UserEntity: IdentityUser<int>
    {
        [StringLength(100)]
        public string? FirstName { get; set; } = string.Empty;
        [StringLength(100)]
        public string? LastName { get; set; } = string.Empty;
        [StringLength(100)]
        public string? Image { get; set; } = string.Empty;

        public ICollection<UserRoleEntity>? UserRoles { get; set; }
    }
}
