using Application.DTOs.BookCopy;
using Application.DTOs.User;
using FluentValidation;

namespace Application.DTOs.Borrowing
{
    public class SubmitBorrowDTO
    {
        public int UserId { get; set; }
        public int BookCopyId { get; set; }
        public string DueDate { get; set; }
        public List<UserListItemDTO> Users { get; set; }
        public List<BookCopyListItemDTO> BookCopies { get; set; }
    }

    public class SubmitBorrowDTOValidator:AbstractValidator<SubmitBorrowDTO>
    {
        public SubmitBorrowDTOValidator()
        {
            RuleFor(u => u.DueDate)
                .NotEmpty()
                .WithName("تاریخ سر رسید")
                .WithMessage("{PropertyName} نمیتواند خالی باشد")
                .MaximumLength(10)
                .WithMessage("{PropertyName} نمی‌تواند بیشتر از {MaxLength} کاراکتر باشد");
        }
    }

    public enum SubmitBorrowResult
    {
        Success,
        DueDatePassedFromNow
    }
}
