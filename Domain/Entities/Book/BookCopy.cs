using Domain.Entities.Common;
using System.ComponentModel.DataAnnotations;

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
        [Display(Name ="موجود")]
        Available,

        [Display(Name = "در امانت")]
        Borrowed,

        [Display(Name = "مفقود")]
        Lost,

        [Display(Name = "معیوب")]
        Damaged
    }
}
