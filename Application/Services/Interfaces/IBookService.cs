using Application.DTOs.Book;

namespace Application.Services.Interfaces
{
    public interface IBookService
    {
        Task<FilterBookDTO> FilterBookAsync(FilterBookDTO filter);
    }
}
