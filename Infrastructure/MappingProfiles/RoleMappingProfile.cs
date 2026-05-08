using Application.DTOs.Role;
using AutoMapper;
using Domain.Entities.Account;

namespace Infrastructure.MappingProfiles
{
    public class RoleMappingProfile:Profile
    {
        public RoleMappingProfile()
        {
            CreateMap<Role,RoleListItemDTO>().ForMember(r=>r.RoleName,r=>r.MapFrom(r=>r.Name));
        }
    }
}