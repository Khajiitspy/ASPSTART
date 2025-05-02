using ASPSTART.Areas.Admin.Models.Users;
using ASPSTART.Data.Entities.Identity;
using AutoMapper;

namespace ASPSTART.Areas.Admin.Mapper
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            CreateMap<UserEntity, UserItemViewModel>()
                .ForMember(x => x.Image, opt => opt.MapFrom(x => x.Image))
                .ReverseMap();

            CreateMap<UserEntity, UserEditViewModel>()
                .ForMember(x => x.Image, opt => opt.MapFrom(x => x.Image))
                .ReverseMap();
        }
    }
}
