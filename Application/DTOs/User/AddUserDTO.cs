using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Application.DTOs.User
{
    public class AddUserDTO
    {
        public string Name { get; set; }
        public string Family { get; set; }
        public string PhoneNumber { get; set; }
        public string MembershipCode { get; set; }
        public string RoleName { get; set; }
        public string Password { get; set; }
        public IFormFile UserAvatarFile { get; set; }

    }

    public class AddUserDTOValidator : AbstractValidator<AddUserDTO>
    {
        public AddUserDTOValidator()
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
                .NotEmpty()
                .WithName("رمز عبور")
                .WithMessage("{PropertyName} را وارد کنید")
                .Length(8, 25)
                .WithMessage("{PropertyName} باید بین {MinLength} و {MaxLength} کاراکتر باشد");

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
