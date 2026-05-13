using Application.DTOs.BookCopy;

namespace Application.Services.Interfaces
{
    public interface IBookCopyService
    {
        Task<FilterBookCopyDTO> FilterBookCopyAsync(FilterBookCopyDTO filter);
    }
}
