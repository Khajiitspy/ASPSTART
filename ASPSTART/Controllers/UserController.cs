using Microsoft.AspNetCore.Mvc;
using ASPSTART.Models.Users;
using ASPSTART.Data;
using AutoMapper;
using ASPSTART.Data.Entities;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Webp;
using ASPSTART.Interfaces;

namespace ASPSTART.Controllers
{
    public class UserController(ASPSTARTDbContext context, IMapper mapper, IImageService imageService) : Controller
    {

        public IActionResult Index() 
        {
            var model = mapper.ProjectTo<UserViewModel>(context.Users).ToList();
            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserCreateViewModel model)
        {
            var entity = await context.Users.FirstOrDefaultAsync(x => x.Name == model.Name);
            if (entity != null)
            {
                ModelState.AddModelError("Name", "Category with this name already exists");
                return View(model);
            }

            entity = mapper.Map<UserEntity>(model);
            entity.Avatar = await imageService.SaveImageAsync(model.Avatar);
            await context.Users.AddAsync(entity);
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var category = await context.Users.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            ViewBag.ImageName = category.Avatar;

            var model = mapper.Map<UserEditViewModel>(category);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var existing = await context.Users.FirstOrDefaultAsync(x => x.Id == model.Id);
            if (existing == null)
            {
                return NotFound();
            }

            var duplicate = await context.Users
                .FirstOrDefaultAsync(x => x.Name == model.Name && x.Id != model.Id);
            if (duplicate != null)
            {
                ModelState.AddModelError("Name", "Another category with this name already exists");
                return View(model);
            }

            existing = mapper.Map(model, existing);

            if (model.Avatar != null)
            {
                await imageService.DeleteImageAsync(existing.Avatar);
                existing.Avatar = await imageService.SaveImageAsync(model.Avatar);
            }
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await context.Users.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            context.Users.Remove(item);
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}

