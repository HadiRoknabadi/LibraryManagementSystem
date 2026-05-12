using Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Book
{
    public class BookAuthor:BaseEntity
    {
        #region Properties

        public int BookId { get; set; }
        public int AuthorId { get; set; }

        #endregion

        #region Relations

        public Book Book { get; set; }
        public Author Author { get; set; }

        #endregion

    }
}
