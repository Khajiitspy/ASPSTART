using ASPSTART.Models.Helpers;
using System.ComponentModel.DataAnnotations;

namespace ASPSTART.Models.Products
{
    public class ProductSearchViewModel
    {
        [Display(Name = "Product Name")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Description")]
        public string Description { get; set; } = string.Empty;

        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 5;

        public int TotalPages { get; set; } = 1;

        public List<SelectItemViewModel> Categories { get; set; } = new ();
    }
}
