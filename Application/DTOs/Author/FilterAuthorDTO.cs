using Application.DTOs.Common;
using Application.DTOs.Paging;

namespace Application.DTOs.Author
{
    public class FilterAuthorDTO:BasePaging
    {
        #region Properties

        public string Name { get; set; }
        public string Family { get; set; }
        public FilterDataOrder OrderBy { get; set; }
        public List<AuthorListItemDTO> Authors { get; set; }

        #endregion

        #region Methods

        public FilterAuthorDTO SetData(List<AuthorListItemDTO> authors)
        {
            this.Authors = authors;
            return this;
        }

        public FilterAuthorDTO SetPaging(BasePaging paging)
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
