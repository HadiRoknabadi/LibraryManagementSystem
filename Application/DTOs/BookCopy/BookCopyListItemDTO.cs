using Domain.Entities.Book;

namespace Application.DTOs.BookCopy
{
    public class BookCopyListItemDTO
    {
        public int Id { get; set; }
        public string BookName { get; set; }
        public string InventoryCode { get; set; }
        public string ShelfLocation { get; set; }
        public BookCopyStatus Status { get; set; }
    }
}
