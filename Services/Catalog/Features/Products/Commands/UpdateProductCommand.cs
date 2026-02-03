using MediatR;
using Catalog.Dtos;
using Catalog.Extensions;

namespace Catalog.Features.Products.Commands
{
    public sealed class UpdateProductCommand : IRequest<ProductDto>
    {
        public UpdateProductRequest Request { get; }

        public UpdateProductCommand(UpdateProductRequest request) => Request = request;
    }

    public sealed class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ProductDto>
    {
        private readonly Catalog.Repository.IProductRepository _repo;

        public UpdateProductCommandHandler(Catalog.Repository.IProductRepository repo) => _repo = repo;

        public async Task<ProductDto> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var entity = request.Request.ToEntity();
            var dto = entity.ToDto();
            return await _repo.UpdateProduct(dto);
        }
    }
}
