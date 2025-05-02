using ASPSTART.Areas.Admin.Models.Users;
using ASPSTART.Constants;
using ASPSTART.Data;
using ASPSTART.Data.Entities;
using ASPSTART.Data.Entities.Identity;
using ASPSTART.Interfaces;
using ASPSTART.Models.Account;
using ASPSTART.Models.Cate;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ASPSTART.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Roles.Admin)]
    public class UsersController(ASPSTARTDbContext context, IImageService imageService, IMapper mapper, UserManager<UserEntity> userManager, RoleManager<RoleEntity> roleManager) : Controller
    {

        public async Task<IActionResult> Index()
        {
            //var model = await userManager.Users
            //    .ProjectTo<UserItemViewModel>(mapper.ConfigurationProvider)
            //    .ToListAsync();

            //return View(model);

            var userRoles = new List<(UserItemViewModel User, IList<string> Roles)>();

            var users = userManager.Users.ToList();

            foreach (var user in users)
            {
                var roles = await userManager.GetRolesAsync(user);
                userRoles.Add((mapper.Map<UserItemViewModel>(user), roles));
            }

            return View(userRoles);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var user = await userManager.FindByIdAsync(id.ToString());
            if (user == null)
                return NotFound();

            var model = mapper.Map<UserEditViewModel>(user);
            model.Id = id;

            var allRoles = roleManager.Roles.Select(r => r.Name!).ToList();
            var userRoles = await userManager.GetRolesAsync(user);

            model.AvailableRoles = allRoles
                .Select(r => new SelectListItem { Text = r, Value = r, Selected = userRoles.Contains(r) })
                .ToList();

            model.SelectedRoles = userRoles.ToList();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserEditViewModel model)
        {
            var user = await userManager.FindByIdAsync(model.Id.ToString());
            if (user == null)
                return NotFound();

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.UserName = model.Email;

            if (model.ImageFile != null && model.ImageFile.Length > 0)
            {
                user.Image = await imageService.SaveImageAsync(model.ImageFile);
            }

            if (!string.IsNullOrEmpty(model.NewPassword))
            {
                var token = await userManager.GeneratePasswordResetTokenAsync(user);
                await userManager.ResetPasswordAsync(user, token, model.NewPassword);
            }

            var userRoles = await userManager.GetRolesAsync(user);
            var rolesToAdd = model.SelectedRoles.Except(userRoles);
            var rolesToRemove = userRoles.Except(model.SelectedRoles);

            await userManager.RemoveFromRolesAsync(user, rolesToRemove);
            await userManager.AddToRolesAsync(user, rolesToAdd);

            var result = await userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            var allRoles = roleManager.Roles.Select(r => r.Name!).ToList();
            model.AvailableRoles = allRoles
                .Select(r => new SelectListItem { Text = r, Value = r, Selected = model.SelectedRoles.Contains(r) })
                .ToList();

            return View(model);
        }
    }
}
