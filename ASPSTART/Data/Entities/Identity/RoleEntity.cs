using Microsoft.AspNetCore.Identity;

namespace ASPSTART.Data.Entities.Identity;
public class RoleEntity : IdentityRole<int>
{
    public ICollection<UserRoleEntity>? UserRoles { get; set; }
}
