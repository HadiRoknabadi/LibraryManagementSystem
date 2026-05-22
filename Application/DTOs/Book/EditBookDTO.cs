using FluentValidation;

namespace Application.DTOs.Book
{
    public class EditBookDTO
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public int PublisherId { get; set; }
        public List<int> AuthorIds { get; set; }
        public string Title { get; set; }
        public string ISBN { get; set; }
    }

    public class EditBookDTOValidator:AbstractValidator<EditBookDTO>
    {
        public EditBookDTOValidator()
        {
            RuleFor(b => b.Title)
                .NotEmpty()
                .WithName("نام کتاب")
                .WithMessage("{PropertyName} را وارد کنید")
                .MaximumLength(300)
                .WithMessage("{PropertyName} نمی‌تواند بیشتر از {MaxLength} کاراکتر باشد");

            RuleFor(b => b.ISBN)
                .NotEmpty()
                .WithName("ISBN")
                .WithMessage("{PropertyName} را وارد کنید")
                .MaximumLength(20)
                .WithMessage("{PropertyName} نمی‌تواند بیشتر از {MaxLength} کاراکتر باشد");

        }
    }

    public enum EditBookResult
    {
        Success,
        NotFound
    }
}
