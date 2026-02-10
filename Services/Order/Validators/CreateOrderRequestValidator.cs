using FluentValidation;
using Order.Dtos;

namespace Order.Validators
{
    public class CreateOrderRequestValidator : AbstractValidator<CreateOrderRequest>
    {
        public CreateOrderRequestValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId is required");
            RuleFor(x => x.FirstName).NotEmpty().Length(2, 50);
            RuleFor(x => x.LastName).NotEmpty().Length(2, 50);
            RuleFor(x => x.EmailAddress).NotEmpty().EmailAddress();
            RuleFor(x => x.AddressLine).NotEmpty().Length(5, 100);
            RuleFor(x => x.Country).NotEmpty().Length(2, 50);
            RuleFor(x => x.State).NotEmpty().Length(2, 50);
            RuleFor(x => x.ZipCode).NotEmpty().Length(3, 20);
            RuleFor(x => x.OrderItems).NotEmpty().WithMessage("Order must have at least one item");
            RuleForEach(x => x.OrderItems).SetValidator(new OrderItemDtoValidator());
        }
    }

    public class OrderItemDtoValidator : AbstractValidator<OrderItemDto>
    {
        public OrderItemDtoValidator()
        {
            RuleFor(x => x.ProductId).NotEmpty();
            RuleFor(x => x.ProductName).NotEmpty();
            RuleFor(x => x.UnitPrice).GreaterThan(0);
            RuleFor(x => x.Quantity).GreaterThan(0);
        }
    }
}
