using MediatR;
using Catalog.Dtos;
using Catalog.Extensions;

namespace Catalog.Features.Products.Commands
{
    public sealed class CreateProductCommand : IRequest<ProductDto>
    {
        public CreateProductRequest Request { get; }

        public CreateProductCommand(CreateProductRequest request) => Request = request;
    }

    public sealed class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductDto>
    {
        private readonly Catalog.Repository.IProductRepository _repo;

        public CreateProductCommandHandler(Catalog.Repository.IProductRepository repo) => _repo = repo;

        public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var entity = request.Request.ToEntity();
            // map to DTO then create via repo
            var dto = entity.ToDto();
            return await _repo.CreateProduct(dto);
        }
    }
}
