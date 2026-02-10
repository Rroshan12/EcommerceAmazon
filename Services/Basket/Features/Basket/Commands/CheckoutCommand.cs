using MediatR;
using Basket.Dtos;
using Basket.Entities;
using Basket.Repositories;
using EventBus.Messages.Events;
using MassTransit;

namespace Basket.Features.Basket.Commands
{
    public sealed class CheckoutCommand : IRequest<bool>
    {
        public BasketCheckoutDto Checkout { get; }
        public CheckoutCommand(BasketCheckoutDto checkout) => Checkout = checkout;
    }

    public sealed class CheckoutCommandHandler : IRequestHandler<CheckoutCommand, bool>
    {
        private readonly IBasketRepository _repo;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<CheckoutCommandHandler> _logger;

        public CheckoutCommandHandler(IBasketRepository repo, IPublishEndpoint publishEndpoint, ILogger<CheckoutCommandHandler> logger)
        {
            _repo = repo;
            _publishEndpoint = publishEndpoint;
            _logger = logger;
        }

        public async Task<bool> Handle(CheckoutCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Get cart
                var cart = await _repo.GetBasketAsync(request.Checkout.UserId);
                if (cart == null) return false;

                // Create checkout event
                var checkoutEvent = new BasketCheckoutEvent
                {
                    UserId = request.Checkout.UserId,
                    FirstName = request.Checkout.UserId,
                    LastName = request.Checkout.UserId,
                    EmailAddress = request.Checkout.UserId,
                    AddressLine = request.Checkout.Street,
                    Country = request.Checkout.Country,
                    State = request.Checkout.State,
                    ZipCode = request.Checkout.ZipCode,
                    TotalPrice = request.Checkout.Total,
                    BasketItems = cart.Items.Select(i => new BasketCheckoutItem
                    {
                        ProductId = i.ProductId,
                        ProductName = i.ProductName,
                        Quantity = i.Quantity,
                        UnitPrice = i.UnitPrice,
                        DiscountAmount = i.DiscountAmount
                    }).ToList()
                };

                // Publish checkout event to RabbitMQ
                _logger.LogInformation("Publishing BasketCheckoutEvent for UserId: {UserId}", request.Checkout.UserId);
                await _publishEndpoint.Publish(checkoutEvent, cancellationToken);

                // Delete cart after checkout
                await _repo.DeleteBasketAsync(cart.Id);

                _logger.LogInformation("Checkout successful for UserId: {UserId}", request.Checkout.UserId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Checkout failed for UserId: {UserId}", request.Checkout.UserId);
                return false;
            }
        }
    }
}

