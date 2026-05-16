using Application.DTOs.Borrowing;
using Application.DTOs.Common;

namespace Application.Services.Interfaces
{
    public interface IBorrowingService
    {
        Task<FilterBorrowingDTO> FilterBorrowingAsync(FilterBorrowingDTO filter);
        Task<ResultDTO<SubmitBorrowResult>> SubmitBorrowAsync(int librarianId,SubmitBorrowDTO submitBorrowDTO);
    }
}
