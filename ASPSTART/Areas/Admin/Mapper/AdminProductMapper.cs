using ASPSTART.Areas.Admin.Models.Products;
using ASPSTART.Data.Entities;
using AutoMapper;
namespace WebSmonder.Mapper;

public class AdminProductMapper : Profile
{
    public AdminProductMapper()
    {
        CreateMap<ProductEntity, AdProductViewModel>()
            .ForMember(x => x.CategoryName, opt => opt.MapFrom(x => x.Category.Name))
            .ForMember(x => x.Images, opt => opt.MapFrom(x => x.ProductImages.Select(x => x.Name)));

        //CreateMap<ProductCreateViewModel, ProductEntity>()
        //    .ForMember(x => x.CategoryName, opt => opt.MapFrom(x => x.Category.Name))
        //    .ForMember(x => x.Images, opt => opt.MapFrom(x => x.ProductImages.Select(x => x.Name)));
    }
}