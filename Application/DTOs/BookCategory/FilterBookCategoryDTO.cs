using Application.DTOs.Common;
using Application.DTOs.Paging;

namespace Application.DTOs.BookCategory
{
    public class FilterBookCategoryDTO:BasePaging
    {
        #region Properties

        public string Title { get; set; }
        public FilterDataOrder OrderBy { get; set; }
        public List<BookCategoryListItemDTO> BookCategories { get; set; }

        #endregion

        #region Methods

        public FilterBookCategoryDTO SetData(List<BookCategoryListItemDTO> bookCategories)
        {
            this.BookCategories = bookCategories;
            return this;
        }

        public FilterBookCategoryDTO SetPaging(BasePaging paging)
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
