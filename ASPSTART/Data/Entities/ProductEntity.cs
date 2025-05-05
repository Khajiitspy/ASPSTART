using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ASPSTART.Data.Entities;

[Table("tblProducts")]
public class ProductEntity
{
    [Key]
    public int Id { get; set; }
    [Required, StringLength(500)]
    public string Name { get; set; }
    [Required, StringLength(4000)]
    public string Description { get; set; }

    [ForeignKey("Category")]
    public int CategoryId { get; set; }
    public CateEntity Category { get; set; }
    public ICollection<ProductImageEntity> ProductImages { get; set; }
}