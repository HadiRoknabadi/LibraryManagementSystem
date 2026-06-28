using Application.DTOs.Borrowing;
using Application.Services.Interfaces;

namespace WebSite.EndPoint.Tests.E2E.Fakes;

public class FakeQuestPdfService : IQuestPDFService
{
    public Task<byte[]> GenerateBorrowingsReportAsync(
        List<BorrowingReportDTO> data)
    {
        return Task.FromResult(
            new byte[] { 1, 2, 3 });
    }
}