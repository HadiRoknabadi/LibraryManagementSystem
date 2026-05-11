using Application.DTOs.Common;
using Application.DTOs.Paging;

namespace Application.DTOs.Publisher
{
    public class FilterPublisherDTO:BasePaging
    {
        #region Properties

        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public FilterDataOrder OrderBy { get; set; }
        public List<PublisherListItemDTO> Publishers { get; set; }

        #endregion

        #region Methods

        public FilterPublisherDTO SetData(List<PublisherListItemDTO> publishers)
        {
            this.Publishers = publishers;
            return this;
        }

        public FilterPublisherDTO SetPaging(BasePaging paging)
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
