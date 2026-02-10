using FluentValidation;
using Order.Dtos;

namespace Order.Validators
{
    public class UpdateOrderRequestValidator : AbstractValidator<UpdateOrderRequest>
    {
        public UpdateOrderRequestValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.FirstName).NotEmpty().Length(2, 50);
            RuleFor(x => x.LastName).NotEmpty().Length(2, 50);
            RuleFor(x => x.EmailAddress).NotEmpty().EmailAddress();
            RuleFor(x => x.AddressLine).NotEmpty().Length(5, 100);
            RuleFor(x => x.Country).NotEmpty().Length(2, 50);
            RuleFor(x => x.State).NotEmpty().Length(2, 50);
            RuleFor(x => x.ZipCode).NotEmpty().Length(3, 20);
        }
    }
}
