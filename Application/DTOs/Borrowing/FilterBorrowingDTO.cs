using Application.DTOs.Common;
using Application.DTOs.Paging;
using Domain.Entities.Book;

namespace Application.DTOs.Borrowing
{
    public class FilterBorrowingDTO:BasePaging
    {
        #region Properties

        public BorrowingStatus Status { get; set; }
        public FilterDataOrder OrderBy { get; set; }
        public List<BorrowingListItemDTO> Borrowings { get; set; }

        #endregion

        #region Methods

        public FilterBorrowingDTO SetData(List<BorrowingListItemDTO> borrowings)
        {
            this.Borrowings = borrowings;
            return this;
        }

        public FilterBorrowingDTO SetPaging(BasePaging paging)
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
