using MediatR;
using Basket.Dtos;
using Basket.Repositories;
using System.Linq;

namespace Basket.Features.Basket.Queries
{
    public sealed class GetBasketQuery : IRequest<ShoppingCartDto>
    {
        public string Id { get; }
        public GetBasketQuery(string id) => Id = id;
    }

    public sealed class GetBasketQueryHandler : IRequestHandler<GetBasketQuery, ShoppingCartDto>
    {
        private readonly IBasketRepository _repo;
        public GetBasketQueryHandler(IBasketRepository repo) => _repo = repo;

        public async Task<ShoppingCartDto> Handle(GetBasketQuery request, CancellationToken cancellationToken)
        {
            var cart = await _repo.GetBasketAsync(request.Id);
            if (cart == null) return null;
            var dto = new ShoppingCartDto { Id = cart.Id, Items = cart.Items.Select(i => new ShoppingCartItemDto { ProductId = i.ProductId, ProductName = i.ProductName, Quantity = i.Quantity, UnitPrice = i.UnitPrice, ImageUrl = i.ImageUrl }).ToList(), TotalPrice = cart.TotalPrice };
            return dto;
        }
    }
}