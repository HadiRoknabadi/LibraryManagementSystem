using Application.DTOs.Account;
using Application.DTOs.Common;
using Application.DTOs.Paging;
using Application.DTOs.User;
using Application.Extensions;
using Application.Generators;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.Entities.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Implementations
{
    public class UserService : IUserService
    {

        #region Constructor

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;

        public UserService(UserManager<User> userManager, SignInManager<User> signInManager, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
        }


        #endregion

        public async Task<FilterUserDTO> FilterUserAsync(FilterUserDTO filter)
        {
            var query = _userManager.Users
                .Include(u => u.UserRoles)
                .ThenInclude(u => u.Role)
                .AsQueryable().AsNoTracking();

            #region State

            switch (filter.State)
            {
                case FilterUserState.All:
                    break;
            }

            #endregion

            #region User Role


            switch (filter.UserRole)
            {
                case FilterUserRole.All:
                    break;

                case FilterUserRole.Admin:
                    query = query.Where(u => u.UserRoles.Any(u => u.Role.Name == "Admin"));
                    break;
            }

            #endregion

            #region Order

            switch (filter.OrderBy)
            {
                case FilterDataOrder.CreateDate_ASC:
                    query = query.OrderBy(u => u.CreateDate);
                    break;

                case FilterDataOrder.CreateDate_DES:
                    query = query.OrderByDescending(u => u.CreateDate);
                    break;
            }

            #endregion

            #region Filter

            if (!string.IsNullOrEmpty(filter.Name))
                query = query.Where(u => EF.Functions.Like(u.Name, $"%{filter.Name}%"));

            if (!string.IsNullOrEmpty(filter.Family))
                query = query.Where(u => EF.Functions.Like(u.Family, $"%{filter.Family}%"));

            if (!string.IsNullOrEmpty(filter.PhoneNumber))
                query = query.Where(u => EF.Functions.Like(u.PhoneNumber, $"%{filter.PhoneNumber}%"));

            #endregion



            #region Paging

            var allEntitiesCount = await query.CountAsync();

            var pager = Pager.Build(filter.PageId, allEntitiesCount, filter.TakeEntity, filter.HowManyShowPageAfterAndBefore);

            var allEntities = _mapper.Map<List<User>, List<UserListItemDTO>>(await query.Paging(pager).ToListAsync());

            #endregion

            return filter.SetPaging(pager).SetData(allEntities);
        }

        public async Task<ResultDTO<GetUserDetailsResult, UserDetailsDTO>> GetUserDetailsAsync(int userId)
        {
            var result = new ResultDTO<GetUserDetailsResult, UserDetailsDTO>
            {
                Status = GetUserDetailsResult.Success,
                Message = "اطلاعات با موفقیت دریافت شد",
                Data = null
            };

            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
            {
                result.Status = GetUserDetailsResult.UserNotFound;
                result.Message = "کاربری یافت نشد";

                return result;
            }

            var userDetails = _mapper.Map<User, UserDetailsDTO>(user);

            result.Data = userDetails;

            return result;
        }

        public async Task<ResultDTO<AddUserResult>> AddUserAsync(AddUserDTO userDTO)
        {
            var result = new ResultDTO<AddUserResult>
            {
                Status = AddUserResult.Success,
                Message = "اطلاعات کاربر با موفقیت ثبت شد"
            };

            if (await IsExistPhoneNumberAsync(userDTO.PhoneNumber) == true)
            {
                result.Status = AddUserResult.PhoneNumberIsExist;
                result.Message = "شماره موبایل وارد شده قبلا در سایت ثبت شده است";

                return result;
            }

            var user = _mapper.Map<AddUserDTO, User>(userDTO);

            //UPLOAD USER AVATAR

            if (userDTO.UserAvatarFile != null)
            {
                var imageName = Generator.GenerateUniqCode() + Path.GetExtension(userDTO.UserAvatarFile.FileName);

                var isImageUploaded = userDTO.UserAvatarFile.AddImageToServer(imageName, PathExtension.UserAvatarOriginRelative, 100, 100, PathExtension.UserAvatarThumbRelative);

                if (isImageUploaded == false)
                {
                    result.Status = AddUserResult.ImageUploadFailed;
                    result.Message = "خطا در آپلود تصویر";

                    return result;
                }

                user.UserAvatar = imageName;
            }
            else
            {
                user.UserAvatar = PathExtension.Default_Avatar_Name;
            }

            var createUserResult = await _userManager.CreateAsync(user, userDTO.Password);

            if (createUserResult.Succeeded == false)
            {
                result.Status = AddUserResult.IdentityError;
                result.Message = createUserResult.Errors.FirstOrDefault().Description;

                return result;
            }

            //Add Role To User

            var addRoleResult = await _userManager.AddToRoleAsync(user, userDTO.RoleName);

            if (addRoleResult.Succeeded == false)
            {
                result.Status = AddUserResult.IdentityError;
                result.Message = addRoleResult.Errors.FirstOrDefault().Description;

                return result;

            }

            return result;
        }

        public async Task<EditUserDTO> GetUserDetailsForEditAsync(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null) return null;

            var userDetails = _mapper.Map<User, EditUserDTO>(user);

            var userRoles = await _userManager.GetRolesAsync(user);

            userDetails.RoleName = userRoles.FirstOrDefault();

            return userDetails;
        }

        public async Task<ResultDTO<EditUserResult>> EditUserAsync(EditUserDTO editUserDTO)
        {
            var result = new ResultDTO<EditUserResult>
            {
                Status = EditUserResult.Success,
                Message = "ویرایش کاربر با موفقیت انجام شد"
            };

            var user = await _userManager.FindByIdAsync(editUserDTO.UserId.ToString());

            if (user == null)
            {
                result.Status = EditUserResult.UserNotFound;
                result.Message = "کاربری با این مشخصات یافت نشد";

                return result;
            }

            if (editUserDTO.PhoneNumber != user.PhoneNumber && await IsExistPhoneNumberAsync(editUserDTO.PhoneNumber) == true)
            {
                result.Status = EditUserResult.PhoneNumberIsExist;
                result.Message = "شماره موبایل وارد شده قبلا در سایت ثبت شده است";

                return result;

            }



            var editedUser = _mapper.Map<EditUserDTO, User>(editUserDTO, user);

            if (editUserDTO.UserAvatarFile != null)
            {
                var userAvatarDeleteFileName = user.UserAvatar != PathExtension.Default_Avatar_Name ? user.UserAvatar : null;

                var imageName = Generator.GenerateUniqCode() + Path.GetExtension(editUserDTO.UserAvatarFile.FileName);

                var isImageUploaded = editUserDTO.UserAvatarFile.AddImageToServer(imageName, PathExtension.UserAvatarOriginRelative, 100, 100, PathExtension.UserAvatarThumbRelative, deleteFileName: userAvatarDeleteFileName);

                if (isImageUploaded == false)
                {
                    result.Status = EditUserResult.ImageUploadFailed;
                    result.Message = "خطا در آپلود تصویر";

                    return result;
                }

                editedUser.UserAvatar = imageName;
            }

            // Remove User Roles And Add New Roles
            var userRole = await _userManager.GetRolesAsync(user);

            var RemoveFromRoleResult = await _userManager.RemoveFromRoleAsync(editedUser, userRole.FirstOrDefault());

            if (RemoveFromRoleResult.Succeeded == false)
            {
                result.Status = EditUserResult.IdentityError;
                result.Message = RemoveFromRoleResult.Errors.FirstOrDefault().Description;

                return result;
            }

            var AddToRoleResult = await _userManager.AddToRoleAsync(editedUser, editUserDTO.RoleName);

            if (AddToRoleResult.Succeeded == false)
            {
                result.Status = EditUserResult.IdentityError;
                result.Message = AddToRoleResult.Errors.FirstOrDefault().Description;

                return result;
            }


            if (!string.IsNullOrEmpty(editUserDTO.Password))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var res = await _userManager.ResetPasswordAsync(user, token, editUserDTO.Password);
            }


            var updateUserResult = await _userManager.UpdateAsync(editedUser);

            if (updateUserResult.Succeeded == false)
            {
                result.Status = EditUserResult.IdentityError;
                result.Message = updateUserResult.Errors.FirstOrDefault().Description;

                return result;
            }


            return result;
        }


        public async Task<ResultDTO<DeleteUserResult>> DeleteUserAsync(int userId)
        {
            var result = new ResultDTO<DeleteUserResult>
            {
                Status = DeleteUserResult.Success,
                Message = "کاربر با موفقیت حذف شد"
            };
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
            {
                result.Status = DeleteUserResult.NotFound;
                result.Message = "کاربری یافت نشد";

                return result;
            }

            user.IsDelete = true;


            var updateUserResult = await _userManager.UpdateAsync(user);

            if (updateUserResult.Succeeded == false)
            {
                result.Status = DeleteUserResult.IdentityError;
                result.Message = updateUserResult.Errors.FirstOrDefault().Description;

                return result;
            }


            return result;
        }

        public async Task<bool> IsExistPhoneNumberAsync(string phoneNumber)
        {
            return await _userManager.Users.AsQueryable().AsNoTracking().AnyAsync(u => u.PhoneNumber == phoneNumber);
        }

        public async Task<ResultDTO<LoginUserResult>> LoginUserAsync(LoginUserDTO loginUserDTO)
        {
            var result = new ResultDTO<LoginUserResult>
            {
                Status=LoginUserResult.Success,
                Message="عملیات ورود  با موفقیت انجام شد"
            };

            var user = await _userManager.FindByNameAsync(loginUserDTO.PhoneNumber);

            if(user== null)
            {
                result.Status = LoginUserResult.UserNotFound;
                result.Message = "کاربری یافت نشد";

                return result;
            }

            var loginResult =await _signInManager.PasswordSignInAsync(user, loginUserDTO.Password,loginUserDTO.RememberMe,false);

            if (loginResult.Succeeded==false)
            {
                result.Status = LoginUserResult.IdentityError;
                result.Message ="عملیات با خطا مواجه شد";

                return result;
            }

            return result;

        }





    }
}
