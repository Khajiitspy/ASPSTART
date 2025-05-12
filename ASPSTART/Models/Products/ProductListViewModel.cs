using ASPSTART.Models.Products;

namespace ASPSTART.Models.Products
{
    public class ProductListViewModel
    {
        public List<ProductItemViewModel> Products { get; set; } = new();
        public ProductSearchViewModel Search { get; set; } = new();

        public int Count { get; set; }
    }
}
