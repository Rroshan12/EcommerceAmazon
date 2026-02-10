using MediatR;
using Basket.Dtos;
using Basket.Entities;
using Basket.Repositories;
using Basket.Services;
using FluentValidation;
using FluentValidation.Results;
using System.Linq;

namespace Basket.Features.Basket.Commands
{
    public sealed class AddItemCommand : IRequest<ShoppingCartDto>
    {
        public string CartId { get; }
        public ShoppingCartItemDto Item { get; }

        public AddItemCommand(string cartId, ShoppingCartItemDto item) => (CartId, Item) = (cartId, item);
    }

    public sealed class AddItemCommandHandler : IRequestHandler<AddItemCommand, ShoppingCartDto>
    {
        private readonly IBasketRepository _repo;
        private readonly IDiscountGrpcService _discountGrpcService;
        private readonly ILogger<AddItemCommandHandler> _logger;

        public AddItemCommandHandler(IBasketRepository repo, IDiscountGrpcService discountGrpcService, ILogger<AddItemCommandHandler> logger)
        {
            _repo = repo;
            _discountGrpcService = discountGrpcService;
            _logger = logger;
        }

        public async Task<ShoppingCartDto> Handle(AddItemCommand request, CancellationToken cancellationToken)
        {
            var cart = await _repo.GetBasketAsync(request.CartId) ?? new ShoppingCart { Id = request.CartId };
            
            var existing = cart.Items.FirstOrDefault(i => i.ProductId == request.Item.ProductId);
            if (existing != null)
            {
                // If validator didn't run or duplicate slipped through, fail with a validation exception
                var failures = new[] { new ValidationFailure("Item", "Item already exists in cart") };
                throw new ValidationException(failures);
            }

            // Fetch discount from Discount gRPC service
            int discountAmount = 0;
            try
            {
                var coupon = await _discountGrpcService.GetDiscountAsync(request.Item.ProductId);
                if (coupon != null)
                {
                    discountAmount = coupon.Amount;
                    _logger.LogInformation("Discount applied for ProductId: {ProductId}, Amount: {Amount}%", request.Item.ProductId, discountAmount);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to fetch discount for ProductId: {ProductId}", request.Item.ProductId);
                // Continue without discount if service fails
            }

            cart.Items.Add(new ShoppingCartItem
            {
                ProductId = request.Item.ProductId,
                ProductName = request.Item.ProductName,
                Quantity = request.Item.Quantity,
                UnitPrice = request.Item.UnitPrice,
                ImageUrl = request.Item.ImageUrl,
                DiscountAmount = discountAmount
            });

            var updated = await _repo.UpdateBasketAsync(cart);
            return new ShoppingCartDto
            {
                Id = updated.Id,
                Items = updated.Items.Select(i => new ShoppingCartItemDto
                {
                    ProductId = i.ProductId,
                    ProductName = i.ProductName,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    ImageUrl = i.ImageUrl,
                    DiscountAmount = i.DiscountAmount
                }).ToList(),
                TotalPrice = updated.TotalPrice
            };
        }
    }
}


