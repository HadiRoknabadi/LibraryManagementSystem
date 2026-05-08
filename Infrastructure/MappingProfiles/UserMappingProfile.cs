using Application.DTOs.Role;
using Application.DTOs.User;
using AutoMapper;
using Domain.Entities.Account;

namespace Infrastructure.MappingProfiles
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {


            CreateMap<User, UserListItemDTO>();

            CreateMap<User, UserDetailsDTO>();


            CreateMap<AddUserDTO, User>()
                .ForMember(u => u.UserName, m => m.MapFrom(c => c.PhoneNumber));


            CreateMap<UserRole, RoleListItemDTO>()
            .ForMember(u => u.Id, u => u.MapFrom(u => u.Role.Id))
            .ForMember(u => u.RoleName, u => u.MapFrom(u => u.Role.Name));

            CreateMap<User, EditUserDTO>()
                .ForMember(u => u.UserId, m => m.MapFrom(u => u.Id));

            CreateMap<EditUserDTO, User>()
                .ForMember(u => u.UserAvatar, m => m.Ignore());
  

        }
    }
}