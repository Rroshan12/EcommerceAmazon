using MediatR;
using Catalog.Dtos;
using Catalog.Repository;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.Features.Types.Queries
{
    public sealed class GetTypesQuery : IRequest<IEnumerable<ProductTypeDto>> { }

    public sealed class GetTypesQueryHandler : IRequestHandler<GetTypesQuery, IEnumerable<ProductTypeDto>>
    {
        private readonly ITypeRepository _repo;

        public GetTypesQueryHandler(ITypeRepository repo) => _repo = repo;

        public async Task<IEnumerable<ProductTypeDto>> Handle(GetTypesQuery request, CancellationToken cancellationToken)
        {
            return await _repo.GetAllTypes();
        }
    }
}
