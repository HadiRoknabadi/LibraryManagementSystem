using Application.DTOs.Borrowing;
using Application.DTOs.Common;
using Domain.Entities.Book;

namespace Application.Services.Interfaces
{
    public interface IBorrowingService
    {
        Task<Borrowing> GetBorrowByIdAsync(int id);
        Task<FilterBorrowingDTO> FilterBorrowingAsync(FilterBorrowingDTO filter);
        Task<ResultDTO<SubmitBorrowResult>> SubmitBorrowAsync(int librarianId,SubmitBorrowDTO submitBorrowDTO);
        Task<ResultDTO<EditBorrowResult>> EditBorrowAsync(EditBorrowDTO editBorrowDTO);
    }
}
