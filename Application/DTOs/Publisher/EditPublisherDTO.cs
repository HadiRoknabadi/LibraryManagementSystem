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
            RuleFor(b => b.Name).NotEmpty().WithMessage("لطفا نام ناشر را وارد کنید")
                .MaximumLength(200).WithMessage("نام ناشر نمیتواند بیشتر از 200 کاراکتر باشد");

            RuleFor(b => b.PhoneNumber)
                .MaximumLength(20).WithMessage("شماره تلفن نمیتواند بیشتر از 20 کاراکتر باشد");

            RuleFor(b => b.Address)
                .MaximumLength(300).WithMessage("آدرس نمیتواند بیشتر از 300 کاراکتر باشد");
        }
    }

    public enum EditPublisherResult
    {
        Success,
        NotFound
    }
}
