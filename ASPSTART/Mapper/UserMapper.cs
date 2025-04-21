using AutoMapper;
using ASPSTART.Data.Entities;
using ASPSTART.Models.Users;
namespace ASPSTART.Mapper;
public class UserMapper : Profile
{
    public UserMapper()
    {
        CreateMap<UserEntity, UserViewModel>()
            .ForMember(x => x.Avatar, opt => opt.MapFrom(x => x.Avatar));
        CreateMap<UserCreateViewModel, UserEntity>()
            .ForMember(x => x.Avatar, opt => opt.Ignore());
        //CreateMap<CateEntity, CateEditViewModel>();
    }
}