using FluentValidation;

namespace Application.DTOs.Book
{
    public class AddBookDTO
    {
        public int CategoryId { get; set; }
        public int PublisherId { get; set; }
        public List<int> AuthorIds { get; set; }
        public string Title { get; set; }
        public string ISBN { get; set; }
    }

    public class AddBookDTOValidator:AbstractValidator<AddBookDTO>
    {
        public AddBookDTOValidator()
        {
            RuleFor(b => b.Title).NotEmpty().WithMessage("لطفا نام کتاب را وارد کنید")
                .MaximumLength(300).WithMessage("نام کتاب نمیتواند بیشتر از 300 کاراکتر باشد");

            RuleFor(b => b.ISBN).NotEmpty().WithMessage("لطفا ISBN را وارد کنید")
                .MaximumLength(20).WithMessage("ISBN نمیتواند بیشتر از 20 کاراکتر باشد");
        }
    }

    public enum AddBookResult
    {
        Success
    }
}
