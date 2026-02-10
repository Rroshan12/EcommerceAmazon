using FluentValidation;
using Basket.Features.Basket.Commands;
using Basket.Repositories;

namespace Basket.Validators
{
    public class AddItemCommandValidator : AbstractValidator<AddItemCommand>
    {
        public AddItemCommandValidator(IBasketRepository repo)
        {
            RuleFor(x => x.CartId).NotEmpty();
            RuleFor(x => x.Item).NotNull().SetValidator(new ShoppingCartItemDtoValidator());

            RuleFor(x => x).MustAsync(async (cmd, ct) =>
            {
                // check duplicates in provided item (can't be duplicate inside single request since only one item)
                // check existing cart if same product already exists
                var cart = await repo.GetBasketAsync(cmd.CartId);
                if (cart == null) return true; // no existing items
                var exists = cart.Items.Any(i => i.ProductId == cmd.Item.ProductId);
                return !exists; // valid when not exists
            }).WithMessage("Item already exists in cart");
        }
    }
}
