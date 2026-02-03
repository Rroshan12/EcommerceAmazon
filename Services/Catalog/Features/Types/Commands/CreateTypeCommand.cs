using MediatR;
using Catalog.Dtos;
using Catalog.Extensions;

namespace Catalog.Features.Types.Commands
{
    public sealed class CreateTypeCommand : IRequest<ProductTypeDto>
    {
        public CreateTypeRequest Request { get; }
        public CreateTypeCommand(CreateTypeRequest request) => Request = request;
    }

    public sealed class CreateTypeCommandHandler : IRequestHandler<CreateTypeCommand, ProductTypeDto>
    {
        private readonly Catalog.Repository.ITypeRepository _repo;
        public CreateTypeCommandHandler(Catalog.Repository.ITypeRepository repo) => _repo = repo;

        public async Task<ProductTypeDto> Handle(CreateTypeCommand request, CancellationToken cancellationToken)
        {
            var dto = new ProductTypeDto { Name = request.Request.Name };
            return await _repo.CreateType(dto);
        }
    }
}
