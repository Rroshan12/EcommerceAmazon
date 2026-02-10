using MediatR;
using Basket.Dtos;
using Basket.Repositories;

namespace Basket.Features.Basket.Commands
{
    public sealed class RemoveItemCommand : IRequest<ShoppingCartDto>
    {
        public string CartId { get; }
        public string ProductId { get; }

        public RemoveItemCommand(string cartId, string productId) => (CartId, ProductId) = (cartId, productId);
    }

    public sealed class RemoveItemCommandHandler : IRequestHandler<RemoveItemCommand, ShoppingCartDto>
    {
        private readonly IBasketRepository _repo;
        public RemoveItemCommandHandler(IBasketRepository repo) => _repo = repo;

        public async Task<ShoppingCartDto> Handle(RemoveItemCommand request, CancellationToken cancellationToken)
        {
            var cart = await _repo.GetBasketAsync(request.CartId);
            if (cart == null) return null;
            var item = cart.Items.FirstOrDefault(i => i.ProductId == request.ProductId);
            if (item != null)
            {
                cart.Items.Remove(item);
            }

            var updated = await _repo.UpdateBasketAsync(cart);
            if (updated == null) return null;
            return new ShoppingCartDto { Id = updated.Id, Items = updated.Items.Select(i => new ShoppingCartItemDto { ProductId = i.ProductId, ProductName = i.ProductName, Quantity = i.Quantity, UnitPrice = i.UnitPrice, ImageUrl = i.ImageUrl }).ToList(), TotalPrice = updated.TotalPrice };
        }
    }
}
