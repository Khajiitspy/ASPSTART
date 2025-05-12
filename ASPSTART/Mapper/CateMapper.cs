using AutoMapper;
using ASPSTART.Data.Entities;
using ASPSTART.Models.Cate;
using ASPSTART.Models.Helpers;
namespace ASPSTART.Mapper;
public class CateMapper : Profile
{
    public CateMapper()
    {
        CreateMap<CateEntity, CateItemViewModel>()
            .ForMember(x => x.Image, opt => opt.MapFrom(x => x.ImageUrl));
        CreateMap<CateCreateViewModel, CateEntity>()
            .ForMember(x => x.ImageUrl, opt => opt.Ignore());
        CreateMap<CateEntity, CateEditViewModel>()
            .ForMember(x => x.ViewImage, opt => opt.MapFrom(x =>
                string.IsNullOrEmpty(x.ImageUrl) ? "/im/default.svg" : $"/images/400_{x.ImageUrl}"))
            .ForMember(x => x.ImageFile, opt => opt.Ignore())
            .ReverseMap();

        CreateMap<CateEntity, SelectItemViewModel>();
    }
}
