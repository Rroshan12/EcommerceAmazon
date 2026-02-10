using FluentValidation;
using Basket.Dtos;

namespace Basket.Validators
{
    public class BasketCheckoutDtoValidator : AbstractValidator<BasketCheckoutDto>
    {
        public BasketCheckoutDtoValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.Total).GreaterThanOrEqualTo(0);
            RuleFor(x => x.City).NotEmpty();
            RuleFor(x => x.Street).NotEmpty();
            RuleFor(x => x.ZipCode).NotEmpty();
        }
    }
}
