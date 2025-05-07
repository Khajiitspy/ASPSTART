using Microsoft.AspNetCore.Mvc;
using ASPSTART.Data;
using AutoMapper;
using ASPSTART.Interfaces;
using ASPSTART.Areas.Admin.Models.Products;
using ASPSTART.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Hosting;
using System.Text.RegularExpressions;
using ASPSTART.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

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
                var html = model.Description;

                var doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(html);

                var imgNodes = doc.DocumentNode.SelectNodes("//img[@src]");
                if (imgNodes != null)
                {
                    foreach (var img in imgNodes)
                    {
                        var src = img.GetAttributeValue("src", null);

                        if (src != null && src.StartsWith("data:image"))
                        {
                            var base64Data = Regex.Match(src, @"data:image/(?<type>.+?);base64,(?<data>.+)").Groups["data"].Value;

                            var fileName = await imageService.SaveImageFromBase64Async(base64Data, false);

                            var imageUrl = Url.Content("/images/" + fileName);
                            img.SetAttributeValue("src", imageUrl);
                        }
                        else if(src != null && src.StartsWith("http"))
                        {
                            var fileName = await imageService.SaveImageFromUrlAsync(src);
                            var imageUrl = Url.Content("/images/" + fileName);
                            img.SetAttributeValue("src", imageUrl);
                        }

                    }

                    model.Description = doc.DocumentNode.OuterHtml;
                }

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

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await context.Products
                .Include(p => p.ProductImages)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (item == null)
            {
                return NotFound();
            }

            var html = item.Description;

            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(html);

            var imgNodes = doc.DocumentNode.SelectNodes("//img[@src]");
            if (imgNodes != null)
            {
                foreach (var img in imgNodes)
                {
                    var src = img.GetAttributeValue("src", null);

                    src = src.Remove(0, src.IndexOf("/images/") + 8);

                    await imageService.DeleteImageAsync(src);
                }
            }

            if (item.ProductImages.Count > 0)
            {
                foreach (var image in item.ProductImages)
                {
                    await imageService.DeleteImageAsync(image.Name);
                    context.ProductImages.Remove(image);
                }
            }

            context.Products.Remove(item);
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
