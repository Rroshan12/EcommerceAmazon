using MediatR;
using Catalog.Dtos;

namespace Catalog.Features.Products.Queries
{
    public sealed class GetProductByIdQuery : IRequest<ProductDto>
    {
        public string Id { get; }

        public GetProductByIdQuery(string id) => Id = id;
    }

    public sealed class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto>
    {
        private readonly Catalog.Repository.IProductRepository _repo;

        public GetProductByIdQueryHandler(Catalog.Repository.IProductRepository repo) => _repo = repo;

        public async Task<ProductDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            return await _repo.GetProduct(request.Id);
        }
    }
}
