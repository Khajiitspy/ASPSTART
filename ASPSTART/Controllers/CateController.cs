using Microsoft.AspNetCore.Mvc;
using ASPSTART.Models.Cate;
using ASPSTART.Data;
using AutoMapper;
using ASPSTART.Data.Entities;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Webp;
using ASPSTART.Interfaces;
using Microsoft.AspNetCore.Authorization;
using ASPSTART.Constants;

namespace ASPSTART.Controllers
{
    public class CateController(ASPSTARTDbContext context, IMapper mapper, IImageService imageService) : Controller
    {

        public IActionResult Index() //Це будь-який web результат - View - сторінка, Файл, PDF, Excel
        {
            var model = mapper.ProjectTo<CateItemViewModel>(context.Categories).ToList();
            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }        
        
        [HttpPost]
        public async Task<IActionResult> Create(CateCreateViewModel model)
        {
            var entity = await context.Categories.FirstOrDefaultAsync(x => x.Name == model.Name);
            if (entity != null)
            {
                ModelState.AddModelError("Name", "Category with this name already exists");
                return View(model);
            }

            entity = mapper.Map<CateEntity>(model);
            entity.ImageUrl = await imageService.SaveImageAsync(model.ImageFile);
            await context.Categories.AddAsync(entity);
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var category = await context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            ViewBag.ImageName = category.ImageUrl;

            var model = mapper.Map<CateEditViewModel>(category);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CateEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var existing = await context.Categories.FirstOrDefaultAsync(x => x.Id == model.Id);
            if (existing == null)
            {
                return NotFound();
            }

            var duplicate = await context.Categories
                .FirstOrDefaultAsync(x => x.Name == model.Name && x.Id != model.Id);
            if (duplicate != null)
            {
                ModelState.AddModelError("Name", "Another category with this name already exists");
                return View(model);
            }

            existing = mapper.Map(model, existing);

            if (model.ImageFile != null)
            {
                await imageService.DeleteImageAsync(existing.ImageUrl);
                existing.ImageUrl = await imageService.SaveImageAsync(model.ImageFile);
            }
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize(Roles = $"{Roles.Admin}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await context.Categories.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            context.Categories.Remove(item);
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
