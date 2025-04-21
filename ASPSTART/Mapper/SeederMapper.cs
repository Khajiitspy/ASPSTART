using AutoMapper;
using ASPSTART.Data.Entities;
using ASPSTART.Models.Seeder;

namespace ASPSTART.Mapper;
public class SeederMapper : Profile
{
    public SeederMapper()
    {
        CreateMap<SeederCateModel, CateEntity>()
            .ForMember(x => x.ImageUrl, opt => opt.MapFrom(x => x.Image));
        CreateMap<SeederUserModel, UserEntity>();
    }
}
