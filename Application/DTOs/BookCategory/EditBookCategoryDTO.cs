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
            RuleFor(b => b.Title).NotEmpty().WithMessage("لطفا نام دسته بندی را وارد کنید")
                .MaximumLength(150).WithMessage("نام دسته بندی نمیتواند بیشتر از 150 کاراکتر باشد");
        }
    }

    public enum EditBookCategoryResult
    {
        Success,
        NotFound
    }

}
