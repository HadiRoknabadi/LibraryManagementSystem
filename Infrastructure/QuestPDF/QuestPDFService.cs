using Application.DTOs.Borrowing;
using Application.Services.Interfaces;
using Application.Utils;
using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Infrastructure.QuestPDF
{
    public class QuestPDFService : IQuestPDFService
    {
        public QuestPDFService()
        {
            FontManager.RegisterFont(File.OpenRead(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/fonts/Vazir.ttf")));
        }

        public Task<byte[]> GenerateBorrowingsReportAsync(List<BorrowingReportDTO> report)
        {

            var pdfBytes = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4.Landscape());
                    page.Margin(1, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.ContentFromRightToLeft();

                    page.DefaultTextStyle(x =>
                        x.FontFamily("Vazirmatn")
                         .FontSize(13)
                    );

                    // ---------------- Header ----------------
                    page.Header()
                        .Column(column =>
                        {
                            column.Item().Row(row =>
                            {
                                row.RelativeItem()
                                    .Text("گزارش امانت کتاب‌ها")
                                    .FontSize(18)
                                    .AlignRight();

                                row.RelativeItem()
                                    .AlignLeft()
                                    .Text($"تاریخ گزارش: {DateTime.Now.ToStringShamsiDateTime():yyyy/MM/dd HH:mm}")
                                    .FontSize(12)
                                    .FontColor("#6c757d");
                            });

                            column.Item()
                                .PaddingTop(5)
                                .LineHorizontal(1)
                                .LineColor(Colors.Black);
                        });

                    // ---------------- Content ----------------
                    page.Content()
                        .PaddingVertical(5, Unit.Millimetre)
                        .Column(column =>
                        {
                            column.Item()
                                .Border(1)
                                .BorderColor("#dee2e6")
                                .Padding(10)
                                .Column(inner =>
                                {
                                    inner.Item().Table(table =>
                                    {
                                        table.ColumnsDefinition(columns =>
                                        {
                                            columns.RelativeColumn(3); // نام کتاب
                                            columns.RelativeColumn(2); // عضو
                                            columns.RelativeColumn(2); // کارمند
                                            columns.RelativeColumn(2); // تاریخ امانت
                                            columns.RelativeColumn(2); // تاریخ سررسید
                                            columns.RelativeColumn(1); // وضعیت
                                        });

                                        // -------- Table Header --------
                                        table.Header(header =>
                                        {
                                            static IContainer HeaderStyle(IContainer container) =>
                                                container
                                                    .DefaultTextStyle(x => x.SemiBold())
                                                    .Padding(6)
                                                    .Background("#f8f9fa")
                                                    .BorderBottom(1)
                                                    .BorderColor("#000000");

                                            header.Cell().Element(HeaderStyle).AlignCenter().Text("نام کتاب");
                                            header.Cell().Element(HeaderStyle).AlignCenter().Text("عضو");
                                            header.Cell().Element(HeaderStyle).AlignCenter().Text("کارمند");
                                            header.Cell().Element(HeaderStyle).AlignCenter().Text("تاریخ امانت");
                                            header.Cell().Element(HeaderStyle).AlignCenter().Text("تاریخ سررسید");
                                            header.Cell().Element(HeaderStyle).AlignCenter().Text("وضعیت");
                                        });

                                        // -------- Table Rows --------
                                        foreach (var item in report)
                                        {
                                            bool isLate = item.Status == "در امانت" && IsLate(item.DueDate);

                                            static IContainer DataStyle(IContainer container) =>
                                                container.Padding(6).BorderBottom(1).BorderColor("#dee2e6");

                                            table.Cell().Element(DataStyle).AlignCenter().Text(item.BookName ?? "");
                                            table.Cell().Element(DataStyle).AlignCenter().Text(item.UserFullName ?? "");
                                            table.Cell().Element(DataStyle).AlignCenter().Text(item.LibrarianFullName ?? "");
                                            table.Cell().Element(DataStyle).AlignCenter().Text(item.BorrowDate ?? "");
                                            table.Cell().Element(DataStyle).AlignCenter().Text(item.DueDate ?? "");

                                            // ستون وضعیت با منطق شرطی
                                            table.Cell().Element(DataStyle).Column(col =>
                                            {
                                                col.Item().AlignCenter().Text(item.Status); // متن اصلی (مثلا: در امانت)

                                                if (isLate)
                                                {
                                                    col.Item().AlignCenter().Text("دارای دیرکرد")
                                                       .FontSize(9) // فونت ریزتر
                                                       .FontColor(Colors.Red.Medium); // رنگ قرمز برای تاکید
                                                }
                                            });
                                        }
                                    });
                                });
                        });

                    // ---------------- Footer ----------------
                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("صفحه ");
                            x.CurrentPageNumber();
                            x.Span(" از ");
                            x.TotalPages();
                        });
                });
            }).GeneratePdf();

            return Task.FromResult(pdfBytes);


        }

        private bool IsLate(string dueDateString)
        {
            if (DateTime.Now > dueDateString.ToMiladiDate()) return true;

            return false;
        }
    }
}
