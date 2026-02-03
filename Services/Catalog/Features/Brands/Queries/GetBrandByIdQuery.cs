using MediatR;
using Catalog.Dtos;
using Catalog.Repository;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.Features.Brands.Queries
{
    public sealed class GetBrandByIdQuery : IRequest<ProductBrandDto>
    {
        public string Id { get; }
        public GetBrandByIdQuery(string id) => Id = id;
    }

    public sealed class GetBrandByIdQueryHandler : IRequestHandler<GetBrandByIdQuery, ProductBrandDto>
    {
        private readonly IBrandRepository _repo;
        public GetBrandByIdQueryHandler(IBrandRepository repo) => _repo = repo;

        public async Task<ProductBrandDto> Handle(GetBrandByIdQuery request, CancellationToken cancellationToken)
        {
            return await _repo.GetByIdAsync(request.Id);
        }
    }
}
