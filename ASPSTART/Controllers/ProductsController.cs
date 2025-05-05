using Microsoft.AspNetCore.Mvc;
using ASPSTART.Data;
using AutoMapper;
using ASPSTART.Interfaces;
using ASPSTART.Models.Product;

namespace ASPSTART.Controllers
{
    public class ProductsController(ASPSTARTDbContext context, IMapper mapper, IImageService imageService) : Controller
    {

        public IActionResult Index() //Це будь-який web результат - View - сторінка, Файл, PDF, Excel
        {
            var model = mapper.ProjectTo<ProductItemViewModel>(context.Products).ToList();
            return View(model);
        }
    }
}
