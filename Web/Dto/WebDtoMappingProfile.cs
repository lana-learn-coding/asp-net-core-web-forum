using AutoMapper;
using Core.Model;
using DAL.Models.Auth;
using Web.Dto.Auth;

namespace Web.Dto
{
    public class WebDtoMappingProfile : Profile
    {
        public WebDtoMappingProfile()
        {
            CreateMap<User, AuthUser>();
            CreateMap<UserView, AuthUser>();
            CreateMap<Role, string>().ConvertUsing(r => r.Name);
        }
    }
}