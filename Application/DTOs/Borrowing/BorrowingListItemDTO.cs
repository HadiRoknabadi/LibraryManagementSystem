using Domain.Entities.Book;

namespace Application.DTOs.Borrowing
{
    public class BorrowingListItemDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserFullName { get; set; }
        public int LibrarianId { get; set; }
        public string LibrarianFullName { get; set; }
        public int BookCopyId { get; set; }
        public int BookId { get; set; }
        public string BookName { get; set; }
        public string BorrowDate { get; set; }
        public string DueDate { get; set; }
        public string ReturnDate { get; set; }
        public BorrowingStatus Status { get; set; }

    }
}
