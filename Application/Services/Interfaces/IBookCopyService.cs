using Application.DTOs.BookCopy;
using Application.DTOs.Common;
using Domain.Entities.Book;

namespace Application.Services.Interfaces
{
    public interface IBookCopyService
    {
        Task<FilterBookCopyDTO> FilterBookCopyAsync(FilterBookCopyDTO filter);
        Task<BookCopy> GetBookCopyByIdAsync(int bookCopyId);
        Task<ResultDTO<AddBookCopyResult>> AddBookCopyAsync(AddBookCopyDTO addBookCopyDTO);
        Task<ResultDTO<EditBookCopyResult>> EditBookCopyAsync(EditBookCopyDTO editBookCopyDTO);
        Task<ResultDTO<DeleteBookCopyResult>> DeleteBookCopyAsync(int bookCopyId);
    }
}
