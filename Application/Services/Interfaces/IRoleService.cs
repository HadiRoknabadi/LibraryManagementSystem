using Application.DTOs.Common;
using Application.DTOs.Role;

namespace Application.Services.Interfaces
{
    public interface IRoleService
    {
        Task<ResultDTO<GetRolesResult, List<RoleListItemDTO>>> GetRolesAsync();
    }
}
