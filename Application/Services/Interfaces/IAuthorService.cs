using Application.DTOs.Author;
using Application.DTOs.Common;
using Domain.Entities.Book;

namespace Application.Services.Interfaces
{
    public interface IAuthorService
    {
        Task<FilterAuthorDTO> FilterAuthorAsync(FilterAuthorDTO filter);
        Task<Author> GetAuthorByIdAsync(int authorId);
        Task<ResultDTO<AddAuthorResult>> AddAuthorAsync(AddAuthorDTO addAuthorDTO);
        Task<ResultDTO<EditAuthorResult>> EditAuthorAsync(EditAuthorDTO editAuthorDTO);
        Task<ResultDTO<DeleteAuthorResult>> DeleteAuthorAsync(int authorId);



    }
}
