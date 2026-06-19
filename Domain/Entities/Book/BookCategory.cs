using Domain.Entities.Common;

namespace Domain.Entities.Book
{
    public class BookCategory:BaseEntity
    {
        #region Properties

        public string Title { get; set; }

        #endregion

        #region Relations

        public ICollection<Book> Books { get; set; }
        #endregion
    }
}
