using FluentValidation;

namespace Application.DTOs.Publisher
{
    public class AddPublisherDTO
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
    }

    public class AddPublisherDTOValidator:AbstractValidator<AddPublisherDTO>
    {
        public AddPublisherDTOValidator()
        {
            RuleFor(b => b.Name)
                .NotEmpty()
                .WithName("نام ناشر")
                .WithMessage("{PropertyName} را وارد کنید")
                .MaximumLength(200)
                .WithMessage("{PropertyName} نمی‌تواند بیشتر از {MaxLength} کاراکتر باشد");

            RuleFor(b => b.PhoneNumber)
                .MaximumLength(20)
                .WithName("شماره تلفن")
                .WithMessage("{PropertyName} نمی‌تواند بیشتر از {MaxLength} کاراکتر باشد");

            RuleFor(b => b.Address)
                .MaximumLength(300)
                .WithName("آدرس")
                .WithMessage("{PropertyName} نمی‌تواند بیشتر از {MaxLength} کاراکتر باشد");

        }
    }

    public enum AddPublisherResult
    {
        Success
    }

}
