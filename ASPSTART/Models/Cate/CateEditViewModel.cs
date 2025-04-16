using System.ComponentModel.DataAnnotations;

namespace ASPSTART.Models.Cate
{
    public class CateEditViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Category Name")]
        public string Name { get; set; } = string.Empty;
        [Display(Name = "Description")]
        public string? Description { get; set; } = string.Empty;

        [Display(Name = "Pick Photo")]
        public IFormFile ImageFile { get; set; } = null!;
    }
}
