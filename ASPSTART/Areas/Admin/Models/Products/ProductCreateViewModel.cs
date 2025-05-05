using Microsoft.AspNetCore.Mvc.Rendering;

namespace ASPSTART.Areas.Admin.Models.Products
{
    public class ProductCreateViewModel
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int CategoryId { get; set; } =  0;
        public List<SelectListItem> AvailableCategories { get; set; } = new();

        public List<IFormFile> Images { get; set; } = new();
        public string ImageOrder { get; set; } = string.Empty;
    }
}
