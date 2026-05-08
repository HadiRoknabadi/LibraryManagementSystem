using Application.DTOs.User;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebSite.EndPoint.Http;
using Application.DTOs.Role;

namespace WebSite.EndPoint.Controllers
{
    public class UserController : BaseController
    {
        #region Constructor

        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

        public UserController(IUserService userService, IRoleService roleService)
        {
            _userService = userService;
            _roleService = roleService;
        }

        #endregion

        #region Users

        [Route("Users")]
        public async Task<IActionResult> Users(FilterUserDTO filter)
        {
            filter.HowManyShowPageAfterAndBefore = 5;
            filter.TakeEntity = 20;
            var result = await _userService.FilterUserAsync(filter);
            return View(result);
        }

        #endregion

        #region Add User

        [Route("AddUser")]
        public async Task<IActionResult> AddUser()
        {
            #region Fill Role List

            var roles=await _roleService.GetRolesAsync();

            ViewData["Roles"] = roles.Status==GetRolesResult.Success?roles.Data:new List<RoleListItemDTO>();

            #endregion

            return View();
        }

        [Route("AddUser")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUser(AddUserDTO userDTO)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.AddUserAsync(userDTO);
                switch (result.Status)
                {
                    case AddUserResult.Success:
                        TempData[Toast_SuccessMessage] = result.Message;
                        return RedirectToAction(nameof(Users));

                    case AddUserResult.PhoneNumberIsExist:
                        TempData[Toast_WarningMessage] = result.Message;
                        break;

                    case AddUserResult.ImageUploadFailed:
                        TempData[Toast_ErrorMessage] = result.Message;
                        break;

                    case AddUserResult.IdentityError:
                        TempData[SweetAlert_ErrorMessage] = result.Message;
                        break;
                }
            }
            #region Fill Role List

            var roles = await _roleService.GetRolesAsync();

            ViewData["Roles"] = roles.Status == GetRolesResult.Success ? roles.Data : new List<RoleListItemDTO>();

            #endregion

            return View(userDTO);
        }

        #endregion

        #region Edit User

        [Route("EditUser/{userId}")]
        public async Task<IActionResult> EditUser(int userId)
        {
            var userDetails = await _userService.GetUserDetailsForEditAsync(userId);

            if (userDetails == null)
            {
                TempData[Toast_WarningMessage] = "کاربری یافت نشد";
                return RedirectToAction(nameof(Users));
            }

            #region Fill Role List

            var roles = await _roleService.GetRolesAsync();

            ViewData["Roles"] = roles.Status == GetRolesResult.Success ? roles.Data : new List<RoleListItemDTO>();

            #endregion

            return View(userDetails);
        }

        [Route("EditUser/{userId?}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(EditUserDTO editUserDTO)
        {
            if (ModelState.IsValid)
            {

                var result = await _userService.EditUserAsync(editUserDTO);

                switch (result.Status)
                {
                    case EditUserResult.Success:
                        TempData[Toast_SuccessMessage] = result.Message;
                        return RedirectToAction(nameof(Users));

                    case EditUserResult.UserNotFound:
                        TempData[Toast_WarningMessage] = result.Message;
                        break;

                    case EditUserResult.PhoneNumberIsExist:
                        TempData[Toast_WarningMessage] = result.Message;
                        break;

                    case EditUserResult.ImageUploadFailed:
                        TempData[SweetAlert_ErrorMessage] = result.Message;
                        break;

                    case EditUserResult.IdentityError:
                        TempData[SweetAlert_ErrorMessage] = result.Message;
                        break;
                }
            }

            #region Fill Role List

            var roles = await _roleService.GetRolesAsync();

            ViewData["Roles"] = roles.Status == GetRolesResult.Success ? roles.Data : new List<RoleListItemDTO>();

            #endregion

            return View(editUserDTO);
        }

        #endregion

        #region Delete User

        [Route("DeleteUser/{userId}")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            var result = await _userService.DeleteUserAsync(userId);
            switch (result.Status)
            {
                case DeleteUserResult.NotFound:
                    return JsonResponseStatus.SendStatus(JsonResponseStatusType.Warning, result.Message, null);

                case DeleteUserResult.Success:
                    return JsonResponseStatus.SendStatus(JsonResponseStatusType.Success, result.Message, null);

                case DeleteUserResult.IdentityError:
                    return JsonResponseStatus.SendStatus(JsonResponseStatusType.Error, result.Message, null);

            }
            return JsonResponseStatus.SendStatus(JsonResponseStatusType.Error, "عملیات مورد نظر با خطا مواجه شد", null);

        }

        #endregion


    }
}
