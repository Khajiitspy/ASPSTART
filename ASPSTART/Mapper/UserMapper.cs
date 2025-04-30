using AutoMapper;
using ASPSTART.Data.Entities;
using ASPSTART.Data.Entities.Identity;
using ASPSTART.Models.Account;
namespace ASPSTART.Mapper;
public class UserMapper : Profile
{
    public UserMapper()
    {
        CreateMap<UserEntity, UserLinkViewModel>()
            .ForMember(x => x.Name, opt =>
                opt.MapFrom(x => $"{x.LastName} {x.FirstName}"))
            .ForMember(x => x.Image, opt =>
                opt.MapFrom(x => x.Image ?? "default.webp"));
    }
}
