using Application.DTOs.Author;
using Application.DTOs.Common;

namespace Application.Services.Interfaces
{
    public interface IAuthorService
    {
        Task<FilterAuthorDTO> FilterAuthorAsync(FilterAuthorDTO filter);
        Task<ResultDTO<AddAuthorResult>> AddAuthorAsync(AddAuthorDTO addAuthorDTO);

    }
}
