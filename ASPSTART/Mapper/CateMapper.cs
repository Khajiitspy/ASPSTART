﻿using AutoMapper;
using ASPSTART.Data.Entities;
using ASPSTART.Models.Cate;
namespace ASPSTART.Mapper;
public class CateMapper : Profile
{
    public CateMapper()
    {
        CreateMap<CateEntity, CateItemViewModel>()
            .ForMember(x => x.Image, opt => opt.MapFrom(x => x.ImageUrl));
        CreateMap<CateCreateViewModel, CateEntity>()
            .ForMember(x => x.ImageUrl, opt => opt.Ignore());
        CreateMap<CateEditViewModel, CateEntity>();
        CreateMap<CateEntity, CateEditViewModel>();
    }
}
