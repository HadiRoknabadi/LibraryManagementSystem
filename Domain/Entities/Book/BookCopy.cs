using Domain.Entities.Common;

namespace Domain.Entities.Book
{
    public class BookCopy:BaseEntity
    {
        #region  Properties

        public int BookId { get; set; }
        public string InventoryCode { get; set; }
        public string ShelfLocation { get; set; }
        public BookCopyStatus Status { get; set; }

        #endregion

        #region Relations

        public Book Book { get; set; }

        #endregion


    }

    public enum BookCopyStatus
    {
        Available,
        Borrowed,
        Lost,
        Damaged
    }
}
