using Application.DTOs.Common;
using Application.DTOs.User;

namespace Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<FilterUserDTO> FilterUserAsync(FilterUserDTO filter);
        Task<ResultDTO<GetUserDetailsResult, UserDetailsDTO>> GetUserDetailsAsync(int userId);
        Task<ResultDTO<AddUserResult>> AddUserAsync(AddUserDTO userDTO);
        Task<EditUserDTO> GetUserDetailsForEditAsync(int userId);
        Task<ResultDTO<EditUserResult>> EditUserAsync(EditUserDTO editUserDTO);
        Task<bool> IsExistPhoneNumberAsync(string phoneNumber);
        Task<ResultDTO<DeleteUserResult>> DeleteUserAsync(int userId);
    }
}
