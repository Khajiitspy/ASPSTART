using ASPSTART.Constants;
using ASPSTART.Data.Entities.Identity;
using ASPSTART.Interfaces;
using ASPSTART.Models.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ASPSTART.Controllers
{
    public class AccountController(UserManager<UserEntity> userManager, SignInManager<UserEntity> signInManager, IImageService imageService) : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                var res = await signInManager.PasswordSignInAsync(user, model.Password, false, false);
                if (res.Succeeded)
                {
                    await signInManager.SignInAsync(user, false);
                    return Redirect("/");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid login attempt");
                    return View(model);
                }
            }
            else
            {
                ModelState.AddModelError("", "Invalid login attempt");
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return Redirect("/");
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {

            var user = new UserEntity
            {
                UserName = model.FirstName,
                Email = model.Email,
                LastName = model.LastName,
                FirstName = model.FirstName,
                Image = await imageService.SaveImageAsync(model.Image)
            };

            var result = await userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                Console.WriteLine($"User {user.FirstName}, {user.LastName} Created");
                await userManager.AddToRoleAsync(user, Roles.Admin);
                return Redirect("/");
            }
            else
            {
                ModelState.AddModelError("", "Invalid login attempt");
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var userName = User.Identity?.Name;
            if (string.IsNullOrEmpty(userName))
            {
                return RedirectToAction("Login");
            }

            var user = await userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return RedirectToAction("Login");
            }

            var model = new ProfileViewModel
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Image = user.Image
            };

            return View(model);
        }
    }
}
