using Domain.Entities.Account;
using Domain.Entities.Common;

namespace Domain.Entities.Book
{
    public class Borrowing:BaseEntity
    {
        #region Properties

        public int UserId { get; set; }
        public int? LibrarianId { get; set; }
        public int BookCopyId { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public BorrowingStatus Status { get; set; }

        #endregion

        #region Relations

        public User User { get; set; }
        public User Librarian { get; set; }

        #endregion
    }

    public enum BorrowingStatus
    {
        Borrowed,
        Returned,
        Late
    }
}
