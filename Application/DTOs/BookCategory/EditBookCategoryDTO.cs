using FluentValidation;

namespace Application.DTOs.BookCategory
{
    public class EditBookCategoryDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }

    }

    public class EditBookCategoryDTOValidator:AbstractValidator<EditBookCategoryDTO>
    {
        public EditBookCategoryDTOValidator()
        {
            RuleFor(b => b.Title)
                .NotEmpty()
                .WithName("نام دسته بندی")
                .WithMessage("{PropertyName} را وارد کنید")
                .MaximumLength(150)
                .WithMessage("{PropertyName} نمی‌تواند بیشتر از {MaxLength} کاراکتر باشد");

        }
    }

    public enum EditBookCategoryResult
    {
        Success,
        NotFound
    }

}
