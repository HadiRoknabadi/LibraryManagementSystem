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
            RuleFor(b => b.Name)
                .NotEmpty()
                .WithName("نام نویسنده")
                .WithMessage("{PropertyName} را وارد کنید")
                .MaximumLength(200)
                .WithMessage("{PropertyName} نمی‌تواند بیشتر از {MaxLength} کاراکتر باشد");

            RuleFor(b => b.Family)
                .NotEmpty()
                .WithName("نام خانوادگی نویسنده")
                .WithMessage("{PropertyName} را وارد کنید")
                .MaximumLength(200)
                .WithMessage("{PropertyName} نمی‌تواند بیشتر از {MaxLength} کاراکتر باشد");

        }
    }

    public enum EditAuthorResult
    {
        Success,
        NotFound
    }


}
