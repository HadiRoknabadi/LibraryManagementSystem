using FluentValidation;

namespace Application.DTOs.Author
{
    public class AddAuthorDTO
    {
        public string Name { get; set; }
        public string Family { get; set; }
    }

    public class AddAuthorDTOValidator:AbstractValidator<AddAuthorDTO>
    {
        public AddAuthorDTOValidator()
        {
            RuleFor(b => b.Name).NotEmpty().WithMessage("لطفا نام نویسنده را وارد کنید")
                .MaximumLength(150).WithMessage("نام نویسنده نمیتواند بیشتر از 200 کاراکتر باشد");

            RuleFor(b => b.Family).NotEmpty().WithMessage("لطفا نام خانوادگی نویسنده را وارد کنید")
                .MaximumLength(150).WithMessage("نام خانوادگی نویسنده نمیتواند بیشتر از 200 کاراکتر باشد");

            RuleFor(b => b.Name).NotEmpty().WithMessage("لطفا نام نویسنده را وارد کنید")
                .MaximumLength(150).WithMessage("نام نویسنده نمیتواند بیشتر از 200 کاراکتر باشد");
        }
    }

    public enum AddAuthorResult
    {
        Success
    }
}
