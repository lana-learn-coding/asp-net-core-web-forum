using AutoMapper;
using Core.Model;
using DAL.Models.Auth;
using Web.Dto.Auth;
using Web.Dto.User;

namespace Web.Dto
{
    public class WebDtoMappingProfile : Profile
    {
        public WebDtoMappingProfile()
        {
            CreateMap<UserView, PublicUser>();
            CreateMap<DAL.Models.Auth.User, AuthUser>();
            CreateMap<UserView, AuthUser>();
            CreateMap<Role, string>().ConvertUsing(r => r.Name);
        }
    }
}