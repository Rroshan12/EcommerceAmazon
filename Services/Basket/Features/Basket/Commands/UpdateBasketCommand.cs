using MediatR;
using Basket.Dtos;
using Basket.Entities;
using Basket.Repositories;
using System.Linq;

namespace Basket.Features.Basket.Commands
{
    public sealed class UpdateBasketCommand : IRequest<ShoppingCartDto>
    {
        public ShoppingCartDto Cart { get; }
        public UpdateBasketCommand(ShoppingCartDto cart) => Cart = cart;
    }

    public sealed class UpdateBasketCommandHandler : IRequestHandler<UpdateBasketCommand, ShoppingCartDto>
    {
        private readonly IBasketRepository _repo;
        public UpdateBasketCommandHandler(IBasketRepository repo) => _repo = repo;

        public async Task<ShoppingCartDto> Handle(UpdateBasketCommand request, CancellationToken cancellationToken)
        {
            var cart = new ShoppingCart { Id = request.Cart.Id, Items = request.Cart.Items.Select(i => new ShoppingCartItem { ProductId = i.ProductId, ProductName = i.ProductName, Quantity = i.Quantity, UnitPrice = i.UnitPrice, ImageUrl = i.ImageUrl }).ToList() };
            var updated = await _repo.UpdateBasketAsync(cart);
            if (updated == null) return null;
            var updatedDto = new ShoppingCartDto { Id = updated.Id, Items = updated.Items.Select(i => new ShoppingCartItemDto { ProductId = i.ProductId, ProductName = i.ProductName, Quantity = i.Quantity, UnitPrice = i.UnitPrice, ImageUrl = i.ImageUrl }).ToList(), TotalPrice = updated.TotalPrice };
            return updatedDto;
        }
    }
}