using AutoMapper;
using DAL.Models.Auth;
using Web.Dto.Auth;

namespace Web.Dto
{
    public class WebDtoMappingProfile : Profile
    {
        public WebDtoMappingProfile()
        {
            CreateMap<User, AuthUser>();
            CreateMap<Role, string>().ConvertUsing(r => r.Name);
        }
    }
}