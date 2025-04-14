using Microsoft.AspNetCore.Mvc;
using ASPSTART.Models.Cate;
using ASPSTART.Data;
using AutoMapper;
using ASPSTART.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ASPSTART.Controllers
{
    public class CateController(ASPSTARTDbContext context, IMapper mapper) : Controller
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
            var item = await context.Categories.FirstOrDefaultAsync(x => x.Name == model.Name);
            if (item != null)
            {
                ModelState.AddModelError("Name", "Category with this name already exists");
                return View(model);
            }

            item = mapper.Map<CateEntity>(model);
            await context.Categories.AddAsync(item);
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

            mapper.Map(model, existing);
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
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
