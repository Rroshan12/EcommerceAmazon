using MediatR;
using Catalog.Dtos;
using Catalog.Extensions;

namespace Catalog.Features.Brands.Commands
{
    public sealed class CreateBrandCommand : IRequest<ProductBrandDto>
    {
        public CreateBrandRequest Request { get; }
        public CreateBrandCommand(CreateBrandRequest request) => Request = request;
    }

    public sealed class CreateBrandCommandHandler : IRequestHandler<CreateBrandCommand, ProductBrandDto>
    {
        private readonly Catalog.Repository.IBrandRepository _repo;
        public CreateBrandCommandHandler(Catalog.Repository.IBrandRepository repo) => _repo = repo;

        public async Task<ProductBrandDto> Handle(CreateBrandCommand request, CancellationToken cancellationToken)
        {
            var dto = new ProductBrandDto
            {
                Name = request.Request.Name
            };

            return await _repo.CreateBrand(dto);
        }
    }
}
