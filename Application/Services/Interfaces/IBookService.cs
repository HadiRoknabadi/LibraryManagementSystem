using Application.DTOs.Book;
using Application.DTOs.Common;
using Domain.Entities.Book;

namespace Application.Services.Interfaces
{
    public interface IBookService
    {
        Task<FilterBookDTO> FilterBookAsync(FilterBookDTO filter);
        Task<ResultDTO<GetBooksResult, List<BookListItemDTO>>> GetAllBooksAsync();
        Task<Book> GetBookByIdAsync(int bookId);
        Task<ResultDTO<AddBookResult>> AddBookAsync(AddBookDTO addBookDTO);
        Task<ResultDTO<GetBookDetailsResult,EditBookDTO>> GetBookDetailsForEditAsync(int bookId);
        Task<ResultDTO<EditBookResult>> EditBookAsync(EditBookDTO editBookDTO);
        Task<ResultDTO<DeleteBookResult>> DeleteBookAsync(int bookId);
    }
}
