using Domain.Entities.Common;

namespace Domain.Entities.Book
{
    public class Book:BaseEntity
    {
        #region Properties

        public int CategoryId { get; set; }
        public int PublisherId { get; set; }
        public string Title { get; set; }
        public string ISBN { get; set; }

        #endregion

        #region Relations

        public BookCategory Category { get; set; }
        public Publisher Publisher { get; set; }

        #endregion

    }
}
