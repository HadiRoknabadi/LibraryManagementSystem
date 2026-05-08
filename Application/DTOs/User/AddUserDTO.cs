using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Application.DTOs.User
{
    public class AddUserDTO
    {
        public string Name { get; set; }
        public string Family { get; set; }
        public string PhoneNumber { get; set; }
        public string RoleName { get; set; }
        public string Password { get; set; }
        public IFormFile UserAvatarFile { get; set; }

    }

    public class AddUserDTOValidator : AbstractValidator<AddUserDTO>
    {
        public AddUserDTOValidator()
        {
            RuleFor(u => u.Name).NotEmpty().WithMessage("لطفا نام را وارد کنید")
              .MaximumLength(200).WithMessage("نام نمی تواند بیشتر از 200 کاراکتر باشد");

            RuleFor(u => u.Family).NotEmpty().WithMessage("لطفا نام خانوادگی را وارد کنید")
                .MaximumLength(200).WithMessage("نام خانوادگی نمی تواند بیشتر از 200 کاراکتر باشد");

            RuleFor(l => l.PhoneNumber).NotEmpty().WithMessage("لطفا شماره موبایل را وارد کنید")
 .MinimumLength(11).WithMessage("شماره موبایل نمی تواند کمتر از 11 کاراکتر باشد")
    .MaximumLength(11).WithMessage("شماره موبایل نمی تواند بیشتر از 11 کاراکتر باشد")
    .Matches(@"^09(0[1-9]|1[0-9]|2[0-9]|3[0-9]|9[0-9]).{7}$").WithMessage("شماره موبایل وارد شده نامعتبر است");

            RuleFor(c => c.Password).NotEmpty().WithMessage("رمز عبور  را وارد کنید").MinimumLength(8)
                .WithMessage("رمز عبور نمی تواند کمتر از 8 کاراکتر باشد")
                .MaximumLength(25).WithMessage("رمز عبور نمی تواند بیشتر از 25 کاراکتر باشد");
        }
    }

    public enum AddUserResult
    {
        Success,
        PhoneNumberIsExist,
        ImageUploadFailed,
        IdentityError
    }
}
