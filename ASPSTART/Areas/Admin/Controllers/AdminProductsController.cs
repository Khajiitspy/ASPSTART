using Microsoft.AspNetCore.Mvc;
using ASPSTART.Data;
using AutoMapper;
using ASPSTART.Interfaces;
using ASPSTART.Areas.Admin.Models.Products;
using ASPSTART.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ASPSTART.Controllers
{
    [Area("Admin")]
    public class AdminProductsController(ASPSTARTDbContext context, IMapper mapper, IImageService imageService) : Controller
    {
        public IActionResult Index()
        {
            var model = mapper.ProjectTo<AdProductViewModel>(context.Products).ToList();
            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var allCategories = context.Categories.ToList();

            var model = new ProductCreateViewModel
            {
                AvailableCategories = allCategories.Select(c => new SelectListItem { Text = c.Name, Value = $"{c.Id}" }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var product = new ProductEntity
                {
                    Name = model.Name,
                    Description = model.Description,
                    CategoryId = model.CategoryId,
                    Category = context.Categories.FirstOrDefault(c => c.Id == model.CategoryId),
                    ProductImages = new List<ProductImageEntity>()
                };
                context.Products.Add(product);
                context.SaveChanges();

                var order = model.ImageOrder.Split(',').Select(int.Parse).ToList();

                for (int i = 0; i < order.Count; i++)
                {
                    var image = model.Images[order[i]];
                    var imageName = await imageService.SaveImageAsync(image);
                    product.ProductImages.Add(new ProductImageEntity { 
                        Name = imageName,
                        Priotity = i,
                        ProductId = product.Id,
                        Product = product
                    });
                }

                await context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(model);
        }
    }
}
