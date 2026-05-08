using Application.DTOs.Common;
using Application.DTOs.Role;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.Entities.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Implementations
{
    public class RoleService : IRoleService
    {

        #region Constructor


        private readonly RoleManager<Role> _roleManager;
        private readonly IMapper _mapper;

        public RoleService(RoleManager<Role> roleManager, IMapper mapper)
        {
            _roleManager = roleManager;
            _mapper = mapper;
        }


        #endregion

        public async Task<ResultDTO<GetRolesResult, List<RoleListItemDTO>>> GetRolesAsync()
        {
            var result = new ResultDTO<GetRolesResult, List<RoleListItemDTO>>
            {
                Status=GetRolesResult.Success,
                Message="نقش ها با موفقیت دریافت شدند",
                Data=null
            };

            var roles=await _roleManager.Roles.ToListAsync();

            if(roles==null)
            {
                result.Status = GetRolesResult.RolesEmpty;
                result.Message = "نقشی وجود ندارد";

                return result;
            }

            result.Data=_mapper.Map<List<Role>,List<RoleListItemDTO>>(roles);

            return result;

        }

    }
}
