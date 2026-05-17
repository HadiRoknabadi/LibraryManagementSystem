using Application.DTOs.BookCopy;
using Application.DTOs.User;
using Domain.Entities.Book;
using FluentValidation;

namespace Application.DTOs.Borrowing
{
    public class EditBorrowDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BookCopyId { get; set; }
        public BorrowingStatus Status { get; set; }
        public string DueDate { get; set; }
        public List<UserListItemDTO> Users { get; set; }
        public List<BookCopyListItemDTO> BookCopies { get; set; }
    }

    public class EditBorrowDTOValidator:AbstractValidator<EditBorrowDTO>
    {
        public EditBorrowDTOValidator()
        {
            RuleFor(u => u.DueDate)
                .NotEmpty()
                .WithName("تاریخ سر رسید")
                .WithMessage("{PropertyName} نمیتواند خالی باشد")
                .MaximumLength(10)
                .WithMessage("{PropertyName} نمی‌تواند بیشتر از {MaxLength} کاراکتر باشد");
        }
    }

    public enum EditBorrowResult
    {
        Success,
        DueDatePassedFromNow,
        NotFound
    }
}
