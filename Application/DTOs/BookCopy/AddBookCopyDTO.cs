using Application.DTOs.Book;
using FluentValidation;

namespace Application.DTOs.BookCopy
{
    public class AddBookCopyDTO
    {
        public int BookId { get; set; }
        public string ShelfLocation { get; set; }
        public List<BookListItemDTO> Books { get; set; }

    }

    public class AddBookCopyDTOValidator:AbstractValidator<AddBookCopyDTO>
    {
        public AddBookCopyDTOValidator()
        {
            RuleFor(b => b.ShelfLocation)
            .MaximumLength(100).WithMessage("موقعیت قفسه نمی‌تواند بیشتر از {MaxLength} کاراکتر باشد");
        }
    }

    public enum AddBookCopyResult
    {
        Success
    }
}
