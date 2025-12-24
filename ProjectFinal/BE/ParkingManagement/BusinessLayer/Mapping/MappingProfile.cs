using AutoMapper;
using BusinessLayer.DTOs.Auth;
using DomainLayer.Entities;

namespace BusinessLayer.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserLoginDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src =>
                    src.UserRoles
                        .Select(ur => ur.Role.RoleName)  
                        .FirstOrDefault() ?? "Guest"     
                ));
        }
    }
}
