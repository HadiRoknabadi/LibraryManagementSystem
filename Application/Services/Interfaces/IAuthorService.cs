using Application.DTOs.Author;

namespace Application.Services.Interfaces
{
    public interface IAuthorService
    {
        Task<FilterAuthorDTO> FilterAuthorAsync(FilterAuthorDTO filter);
    }
}
