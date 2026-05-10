using FluentValidation;

namespace Application.DTOs.Author
{
    public class EditAuthorDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Family { get; set; }
    }

    public class EditAuthorDTOValidator: AbstractValidator<EditAuthorDTO>
    {
        public EditAuthorDTOValidator()
        {
            RuleFor(b => b.Name).NotEmpty().WithMessage("لطفا نام نویسنده را وارد کنید")
                .MaximumLength(200).WithMessage("نام نویسنده نمیتواند بیشتر از 200 کاراکتر باشد");

            RuleFor(b => b.Family).NotEmpty().WithMessage("لطفا نام خانوادگی نویسنده را وارد کنید")
                .MaximumLength(200).WithMessage("نام خانوادگی نویسنده نمیتواند بیشتر از 200 کاراکتر باشد");
        }
    }

    public enum EditAuthorResult
    {
        Success,
        NotFound
    }


}
