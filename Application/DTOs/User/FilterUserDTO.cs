using Application.DTOs.Common;
using Application.DTOs.Paging;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.User
{
    public class FilterUserDTO:BasePaging
    {
        #region Properties

        public int UserId { get; set; }
        public string Name { get; set; }
        public string Family { get; set; }
        public string PhoneNumber { get; set; }
        public FilterUserState State { get; set; }
        public FilterUserRole UserRole { get; set; }
        public FilterDataOrder OrderBy { get; set; }
        public List<UserListItemDTO> Users { get; set; }


        #endregion

        #region Methods

        public FilterUserDTO SetData(List<UserListItemDTO> users)
        {
            this.Users = users;
            return this;
        }

        public FilterUserDTO SetPaging(BasePaging paging)
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

    public enum FilterUserState
    {
        [Display(Name = "همه")]
        All
    }

    public enum FilterUserRole
    {
        [Display(Name = "همه")]
        All,

        [Display(Name = "مدیر")]
        Admin
    }
}
