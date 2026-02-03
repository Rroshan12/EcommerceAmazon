using MediatR;
using Catalog.Dtos;
using Catalog.Extensions;

namespace Catalog.Features.Brands.Commands
{
    public sealed class UpdateBrandCommand : IRequest<ProductBrandDto>
    {
        public UpdateBrandRequest Request { get; }
        public UpdateBrandCommand(UpdateBrandRequest request) => Request = request;
    }

    public sealed class UpdateBrandCommandHandler : IRequestHandler<UpdateBrandCommand, ProductBrandDto>
    {
        private readonly Catalog.Repository.IBrandRepository _repo;
        public UpdateBrandCommandHandler(Catalog.Repository.IBrandRepository repo) => _repo = repo;

        public async Task<ProductBrandDto> Handle(UpdateBrandCommand request, CancellationToken cancellationToken)
        {
            var dto = new ProductBrandDto { Id = request.Request.Id, Name = request.Request.Name };
            return await _repo.UpdateBrand(dto);
        }
    }
}
