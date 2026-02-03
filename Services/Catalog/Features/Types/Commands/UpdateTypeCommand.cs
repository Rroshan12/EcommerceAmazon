using MediatR;
using Catalog.Dtos;
using Catalog.Extensions;

namespace Catalog.Features.Types.Commands
{
    public sealed class UpdateTypeCommand : IRequest<ProductTypeDto>
    {
        public UpdateTypeRequest Request { get; }
        public UpdateTypeCommand(UpdateTypeRequest request) => Request = request;
    }

    public sealed class UpdateTypeCommandHandler : IRequestHandler<UpdateTypeCommand, ProductTypeDto>
    {
        private readonly Catalog.Repository.ITypeRepository _repo;
        public UpdateTypeCommandHandler(Catalog.Repository.ITypeRepository repo) => _repo = repo;

        public async Task<ProductTypeDto> Handle(UpdateTypeCommand request, CancellationToken cancellationToken)
        {
            var dto = new ProductTypeDto { Id = request.Request.Id, Name = request.Request.Name };
            return await _repo.UpdateType(dto);
        }
    }
}
