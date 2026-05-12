using Application.DTOs.Book;
using Application.DTOs.Common;

namespace Application.Services.Interfaces
{
    public interface IBookService
    {
        Task<FilterBookDTO> FilterBookAsync(FilterBookDTO filter);
        Task<ResultDTO<AddBookResult>> AddBookAsync(AddBookDTO addBookDTO);
    }
}
