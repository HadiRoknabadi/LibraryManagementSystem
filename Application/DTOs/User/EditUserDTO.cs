using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Application.DTOs.User
{
    public class EditUserDTO
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Family { get; set; }
        public string PhoneNumber { get; set; }
        public string MembershipCode { get; set; }
        public string RoleName { get; set; }
        public string Password { get; set; }
        public string UserAvatar { get; set; }
        public IFormFile UserAvatarFile { get; set; }


    }

    public class EditUserDTOValidator:AbstractValidator<EditUserDTO>
    {
        public EditUserDTOValidator()
        {
            RuleFor(u => u.Name)
                .NotEmpty()
                .WithName("نام")
                .WithMessage("{PropertyName} را وارد کنید")
                .MaximumLength(200)
                .WithMessage("{PropertyName} نمی‌تواند بیشتر از {MaxLength} کاراکتر باشد");

            RuleFor(u => u.Family)
                .NotEmpty()
                .WithName("نام خانوادگی")
                .WithMessage("{PropertyName} را وارد کنید")
                .MaximumLength(200)
                .WithMessage("{PropertyName} نمی‌تواند بیشتر از {MaxLength} کاراکتر باشد");

            RuleFor(l => l.PhoneNumber)
                .NotEmpty()
                .WithName("شماره موبایل")
                .WithMessage("{PropertyName} را وارد کنید")
                .Length(11)
                .WithMessage("{PropertyName} باید دقیقا {MinLength} کاراکتر باشد")
                .Matches(@"^09\d{9}$")
                .WithMessage("{PropertyName} وارد شده نامعتبر است");

            RuleFor(u => u.MembershipCode)
                .MaximumLength(20)
                .WithName("کد عضویت")
                .WithMessage("{PropertyName} نمی‌تواند بیشتر از {MaxLength} کاراکتر باشد");

            RuleFor(c => c.Password)
                .Length(8, 25)
                .WithName("رمز عبور")
                .WithMessage("{PropertyName} باید بین {MinLength} و {MaxLength} کاراکتر باشد");


        }
    }


    public enum EditUserResult
    {
        Success,
        UserNotFound,
        PhoneNumberIsExist,
        ImageUploadFailed,
        IdentityError
    }
}
