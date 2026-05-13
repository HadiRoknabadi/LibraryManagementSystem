using Application.DTOs.Common;
using Application.DTOs.Paging;
using Domain.Entities.Book;

namespace Application.DTOs.BookCopy
{
    public class FilterBookCopyDTO:BasePaging
    {
        #region Properties

        public string BookName { get; set; }
        public string InventoryCode { get; set; }
        public BookCopyStatus Status { get; set; }
        public FilterDataOrder OrderBy { get; set; }
        public List<BookCopyListItemDTO> BookCopies { get; set; }

        #endregion


        #region Methods

        public FilterBookCopyDTO SetData(List<BookCopyListItemDTO> bookCopies)
        {
            this.BookCopies = bookCopies;
            return this;
        }

        public FilterBookCopyDTO SetPaging(BasePaging paging)
        {
            this.PageId = paging.PageId;
            this.AllEntitiesCount = paging.AllEntitiesCount;
            this.StartPage = paging.StartPage;
            this.EndPage = paging.EndPage;
            this.HowManyShowPageAfterAndBefore = paging.HowManyShowPageAfterAndBefore;
            this.TakeEntity = paging.TakeEntity;
            this.SkipEntity = paging.SkipEntity;
            this.PageCount = paging.PageCount;

            return this;
        }
        #endregion

    }
}
