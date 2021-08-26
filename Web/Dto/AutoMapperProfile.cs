using AutoMapper;
using DAL.Models.Auth;
using Web.Dto.Auth;

namespace Web.Dto
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, AuthUser>();
            CreateMap<Role, string>().ConvertUsing(r => r.Name);
        }
    }
}