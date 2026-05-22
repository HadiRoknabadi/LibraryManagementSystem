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
            RuleFor(l => l.PhoneNumber)
                .NotEmpty()
                .WithName("شماره موبایل")
                .WithMessage("{PropertyName} را وارد کنید")
                .Length(11)
                .WithMessage("{PropertyName} باید دقیقا {MinLength} کاراکتر باشد")
                .Matches(@"^09(0[1-9]|1[0-9]|2[0-9]|3[0-9]|9[0-9]).{7}$")
                .WithMessage("{PropertyName} وارد شده نامعتبر است");

            RuleFor(c => c.Password)
                .NotEmpty()
                .WithName("رمز عبور")
                .WithMessage("{PropertyName} را وارد کنید")
                .Length(8, 25)
                .WithMessage("{PropertyName} باید بین {MinLength} و {MaxLength} کاراکتر باشد");


        }
    }

    public enum LoginUserResult
    {
        Success,
        UserNotFound,
        IdentityError
    }

}
