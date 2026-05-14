using Application.DTOs.Book;
using FluentValidation;

namespace Application.DTOs.BookCopy
{
    public class EditBookCopyDTO
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public string ShelfLocation { get; set; }
        public List<BookListItemDTO> Books { get; set; }
    }

    public class EditBookCopyDTOValidator:AbstractValidator<EditBookCopyDTO>
    {
        public EditBookCopyDTOValidator()
        {
            RuleFor(b => b.ShelfLocation)
                .MaximumLength(100)
                .WithName("موقعیت قفسه")
                .WithMessage("{PropertyName} نمی‌تواند بیشتر از {MaxLength} کاراکتر باشد");
        }
    }

    public enum EditBookCopyResult
    {
        Success,
        NotFound
    }

}
