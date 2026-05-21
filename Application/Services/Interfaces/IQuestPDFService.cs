using Application.DTOs.Borrowing;

namespace Application.Services.Interfaces
{
    public interface IQuestPDFService
    {
        Task<byte[]> GenerateBorrowingsReportAsync(List<BorrowingReportDTO> report);
    }
}
