using MediatR;
using Catalog.Dtos;
using Catalog.Repository;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.Features.Types.Queries
{
    public sealed class GetTypeByIdQuery : IRequest<ProductTypeDto>
    {
        public string Id { get; }
        public GetTypeByIdQuery(string id) => Id = id;
    }

    public sealed class GetTypeByIdQueryHandler : IRequestHandler<GetTypeByIdQuery, ProductTypeDto>
    {
        private readonly ITypeRepository _repo;
        public GetTypeByIdQueryHandler(ITypeRepository repo) => _repo = repo;

        public async Task<ProductTypeDto> Handle(GetTypeByIdQuery request, CancellationToken cancellationToken)
        {
            return await _repo.GetByIdAsync(request.Id);
        }
    }
}
