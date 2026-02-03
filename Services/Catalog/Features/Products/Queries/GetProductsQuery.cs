using MediatR;
using Catalog.Dtos;
using Catalog.Specifications;

namespace Catalog.Features.Products.Queries
{
    public sealed class GetProductsQuery : IRequest<PaginationDto<ProductDto>>
    {
        public CatalogSpecParams SpecParams { get; }

        public GetProductsQuery(CatalogSpecParams specParams)
        {
            SpecParams = specParams;
        }
    }

    public sealed class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, PaginationDto<ProductDto>>
    {
        private readonly Catalog.Repository.IProductRepository _repo;

        public GetProductsQueryHandler(Catalog.Repository.IProductRepository repo)
        {
            _repo = repo;
        }

        public async Task<PaginationDto<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            return await _repo.GetAllAsync(request.SpecParams);
        }
    }
}
