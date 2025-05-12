using Microsoft.AspNetCore.Mvc;
using ASPSTART.Data;
using AutoMapper;
using ASPSTART.Interfaces;
using ASPSTART.Models.Products;
using ASPSTART.Models.Helpers;
using Microsoft.EntityFrameworkCore;

namespace ASPSTART.Controllers
{
    public class ProductsController(ASPSTARTDbContext context, IMapper mapper, IImageService imageService) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index(ProductSearchViewModel searchModel) //Це будь-який web результат - View - сторінка, Файл, PDF, Excel
        {
            ViewBag.Title = "Products";

            searchModel.Categories = await mapper.ProjectTo<SelectItemViewModel>(context.Categories)
                .ToListAsync();

            var query = context.Products.AsQueryable();

            if(!string.IsNullOrEmpty(searchModel.Name))
            {
                string textSearch = searchModel.Name.Trim();
                query = query.Where(p => p.Name.ToLower().Contains(textSearch.ToLower()));
            }

            if (searchModel.CategoryId !=0)
            {
                query = query.Where(p => p.CategoryId == searchModel.CategoryId);
            }

            if (!string.IsNullOrEmpty(searchModel.Description))
            {
                string textSearch = searchModel.Description.Trim();
                query = query.Where(p => p.Description.ToLower().Contains(textSearch.ToLower()));
            }


            var model = new ProductListViewModel();

            model.Count = query.Count();

            model.Products = mapper.ProjectTo<ProductItemViewModel>(query).ToList();
            model.Search = searchModel;

            model.Search.TotalPages = (int)Math.Ceiling((double)model.Count / model.Search.PageSize);

            return View(model);
        }
    }
}
