using MediatR;
using Catalog.Dtos;
using Catalog.Repository;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.Features.Brands.Queries
{
    public sealed class GetBrandsQuery : IRequest<IEnumerable<ProductBrandDto>> { }

    public sealed class GetBrandsQueryHandler : IRequestHandler<GetBrandsQuery, IEnumerable<ProductBrandDto>>
    {
        private readonly IBrandRepository _repo;

        public GetBrandsQueryHandler(IBrandRepository repo) => _repo = repo;

        public async Task<IEnumerable<ProductBrandDto>> Handle(GetBrandsQuery request, CancellationToken cancellationToken)
        {
            return await _repo.GetAllBrands();
        }
    }
}
