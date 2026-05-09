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
            RuleFor(b => b.Title).NotEmpty().WithMessage("لطفا نام دسته بندی را وارد کنید")
                .MaximumLength(150).WithMessage("نام دسته بندی نمیتواند بیشتر از 150 کاراکتر باشد");
        }
    }

    public enum AddBookCategoryResult
    {
        Success
    }
}
