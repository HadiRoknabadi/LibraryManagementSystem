using WebSite.EndPoint.PresentationExtensions;
using Microsoft.AspNetCore.Mvc;

namespace WebSite.EndPoint.ViewComponents
{
    #region Admin Header
    public class AdminHeaderViewComponent : ViewComponent
    {
        #region Constructor


        #endregion

        public async Task<IViewComponentResult> InvokeAsync()
        {
            //var currentUserId = User.GetUserId();
            //var result = await _userService.GetUserDetailsAsync(currentUserId);
            //ViewData["UserFullName"]=result.Data.FullName;
            //ViewData["UserAvatar"]= result.Data.UserAvatar;
            return View("AdminHeader");
        }

    }

    #endregion


}
