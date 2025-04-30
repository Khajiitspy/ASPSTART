using ASPSTART.Constants;
using ASPSTART.Data;
using ASPSTART.Data.Entities.Identity;
using ASPSTART.Models.Cate;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ASPSTART.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Roles.Admin)]
    public class UsersController(ASPSTARTDbContext context, IMapper mapper, UserManager<UserEntity> userManager) : Controller
    {

        public async Task<IActionResult> Index()
        {
            var users = userManager.Users.ToList();

            var userRoles = new List<(UserEntity User, IList<string> Roles)>();

            foreach (var user in users)
            {
                var roles = await userManager.GetRolesAsync(user);
                userRoles.Add((user, roles));
            }

            return View(userRoles);
        }
    }
}
