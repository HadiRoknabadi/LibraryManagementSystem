using Application.DTOs.BookCopy;
using Application.DTOs.Common;

namespace Application.Services.Interfaces
{
    public interface IBookCopyService
    {
        Task<FilterBookCopyDTO> FilterBookCopyAsync(FilterBookCopyDTO filter);
        Task<ResultDTO<AddBookCopyResult>> AddBookCopyAsync(AddBookCopyDTO addBookCopyDTO);
    }
}
