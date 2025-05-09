using ASPSTART.Constants;
using ASPSTART.Data.Entities.Identity;
using ASPSTART.Interfaces;
using ASPSTART.Models.Account;
using ASPSTART.SMTP;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebSmonder.Services;

namespace ASPSTART.Controllers
{
    public class AccountController(UserManager<UserEntity> userManager, SignInManager<UserEntity> signInManager, IImageService imageService, ISMTPService sMTPService) : Controller
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
                FirstName = model.FirstName
            };

            if (model.Image.Length > 0)
            {
                user.Image = await imageService.SaveImageAsync(model.Image);
            }
            else
            {
                user.Image = "default.webp";
            }

            var result = await userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                Console.WriteLine($"User {user.FirstName}, {user.LastName} Created");
                await userManager.AddToRoleAsync(user, Roles.User);

                // Logging the user in after sign up
                user = await userManager.FindByEmailAsync(model.Email); // Making sure it gets the user I just added
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

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await userManager.FindByEmailAsync(model.Email);
            if (user==null)
            {
                ModelState.AddModelError("", "Invalid email");
                return View(model);
            }

            var token = await userManager.GeneratePasswordResetTokenAsync(user);

            var resetUrl = Url.Action(
                "ResetPassword",
                "Account",
                new { email = user.Email, token },
                protocol: Request.Scheme);

            Message msgEmail = new Message
            {
                Body = $"Reset Password <a href='{resetUrl}'>Reset Password</a>",
                Subject = $"Sending Password",
                To = model.Email,
            };

            var result = await sMTPService.SendMessageAsync(msgEmail);

            if (!result)
            {
                ModelState.AddModelError("", "Error sending email");
                return View(model);
            }

            return RedirectToAction(nameof(ForgotPasswordSend));
        }

        [HttpGet]
        public IActionResult ForgotPasswordSend()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ResetPassword(string email, string token)
        {
            var model = new ResetPasswordViewModel
            {
                Email = email,
                Token = token
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            if(model.Password != model.ConfirmPassword)
            {
                ModelState.AddModelError("", "Passwords do not match");
                return View(model);
            }
            else
            {
                var user = await userManager.FindByEmailAsync(model.Email);

                if(user == null)
                {
                    ModelState.AddModelError("", "Invalid email");
                    return View(model);
                }

                var result = await userManager.ResetPasswordAsync(user, model.Token, model.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Login));
                }
                else
                {
                    ModelState.AddModelError("", $"{result.ToString()}");
                    return View(model);
                }
            }
        }
    }
}
