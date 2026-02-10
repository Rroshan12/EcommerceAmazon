using FluentValidation;
using Basket.Dtos;

namespace Basket.Validators
{
    public class ShoppingCartDtoValidator : AbstractValidator<ShoppingCartDto>
    {
        public ShoppingCartDtoValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleForEach(x => x.Items).SetValidator(new ShoppingCartItemDtoValidator());
        }
    }
}
