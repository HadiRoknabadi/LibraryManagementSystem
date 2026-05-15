using Application.DTOs.Borrowing;

namespace Application.Services.Interfaces
{
    public interface IBorrowingService
    {
        Task<FilterBorrowingDTO> FilterBorrowingAsync(FilterBorrowingDTO filter);
    }
}
