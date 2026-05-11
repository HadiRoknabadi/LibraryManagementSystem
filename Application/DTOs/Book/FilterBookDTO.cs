using Application.DTOs.Common;
using Application.DTOs.Paging;

namespace Application.DTOs.Book
{
    public class FilterBookDTO:BasePaging
    {
        #region Properties

        public string Title { get; set; }
        public string ISBN { get; set; }
        public FilterDataOrder OrderBy { get; set; }
        public List<BookListItemDTO> Books { get; set; }

        #endregion

        #region Methods

        public FilterBookDTO SetData(List<BookListItemDTO> books)
        {
            this.Books = books;
            return this;
        }

        public FilterBookDTO SetPaging(BasePaging paging)
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
