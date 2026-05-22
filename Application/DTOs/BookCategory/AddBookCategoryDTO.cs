using FluentValidation;

namespace Application.DTOs.BookCategory
{
    public class AddBookCategoryDTO
    {
        public string Title { get; set; }

    }

    public class AddBookCategoryDTOValidator:AbstractValidator<AddBookCategoryDTO>
    {
        public AddBookCategoryDTOValidator()
        {
            RuleFor(b => b.Title)
                .NotEmpty()
                .WithName("نام دسته بندی")
                .WithMessage("{PropertyName} را وارد کنید")
                .MaximumLength(150)
                .WithMessage("{PropertyName} نمی‌تواند بیشتر از {MaxLength} کاراکتر باشد");

        }
    }

    public enum AddBookCategoryResult
    {
        Success
    }
}
