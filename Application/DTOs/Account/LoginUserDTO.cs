using FluentValidation;

namespace Application.DTOs.Account
{
    public class LoginUserDTO
    {
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }

    public class LoginUserDTOValidator:AbstractValidator<LoginUserDTO>
    {
        public LoginUserDTOValidator()
        {
            RuleFor(l => l.PhoneNumber).NotEmpty().WithMessage("لطفا شماره موبایل را وارد کنید")
            .MinimumLength(11).WithMessage("شماره موبایل نمی تواند کمتر از 11 کاراکتر باشد")
            .MaximumLength(11).WithMessage("شماره موبایل نمی تواند بیشتر از 11 کاراکتر باشد")
            .Matches(@"^09(0[1-9]|1[0-9]|2[0-9]|3[0-9]|9[0-9]).{7}$").WithMessage("شماره موبایل وارد شده نامعتبر است");

            RuleFor(c => c.Password).NotEmpty().WithMessage("رمز عبور  را وارد کنید").MinimumLength(8)
            .WithMessage("رمز عبور نمی تواند کمتر از 8 کاراکتر باشد")
            .MaximumLength(25).WithMessage("رمز عبور نمی تواند بیشتر از 25 کاراکتر باشد");


        }
    }

    public enum LoginUserResult
    {
        Success,
        UserNotFound,
        IdentityError
    }

}
