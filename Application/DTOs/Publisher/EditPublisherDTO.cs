using FluentValidation;


namespace Application.DTOs.Publisher
{
    public class EditPublisherDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
    }

    public class EditPublisherDTOValidator: AbstractValidator<EditPublisherDTO>
    {
        public EditPublisherDTOValidator()
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

    public enum EditPublisherResult
    {
        Success,
        NotFound
    }
}
