using MediatR;
using Catalog.Dtos;

namespace Catalog.Features.Products.Commands
{
    public sealed class DeleteProductCommand : IRequest<ProductDto>
    {
        public string Id { get; }

        public DeleteProductCommand(string id) => Id = id;
    }

    public sealed class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, ProductDto>
    {
        private readonly Catalog.Repository.IProductRepository _repo;

        public DeleteProductCommandHandler(Catalog.Repository.IProductRepository repo) => _repo = repo;

        public async Task<ProductDto> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            return await _repo.DeleteProduct(request.Id);
        }
    }
}
