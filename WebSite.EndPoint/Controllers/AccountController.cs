using Application.DTOs.Account;
using Application.Services.Interfaces;
using DNTCaptcha.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebSite.EndPoint.Controllers
{
    public class AccountController : BaseController
    {
        #region Constructor

        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        #endregion

        #region Login

        [Route("Login")]
        public IActionResult Login(string returnUrl)
        {
            if(User.Identity.IsAuthenticated)
            {
                return RedirectToAction(controllerName: "Home", actionName: "Dashboard");
            }

            ViewData["ReturnUrl"] = returnUrl;

            return View();
        }

        [Route("Login")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateDNTCaptcha(ErrorMessage = "کد امنیتی وارد شده اشتباه بود")]
        public async Task<IActionResult> Login(LoginUserDTO loginUserDTO, string returnUrl)
        {
            if(ModelState.IsValid)
            {
                var result=await _userService.LoginUserAsync(loginUserDTO);

                switch (result.Status)
                {
                    case LoginUserResult.Success:
                        TempData[Toast_SuccessMessage] = result.Message;
                        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl)) return Redirect(returnUrl);
                        return RedirectToAction(controllerName: "Home", actionName: "Dashboard");

                    case LoginUserResult.UserNotFound:
                        TempData[Toast_WarningMessage] = result.Message;
                        break;

                    case LoginUserResult.IdentityError:
                        TempData[SweetAlert_ErrorMessage]= result.Message;
                        break;
                }
            }

            return View(loginUserDTO);
        }

        #endregion
    }
}
