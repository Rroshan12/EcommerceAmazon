using MediatR;
using Basket.Repositories;

namespace Basket.Features.Basket.Commands
{
    public sealed class DeleteBasketCommand : IRequest<bool>
    {
        public string Id { get; }
        public DeleteBasketCommand(string id) => Id = id;
    }

    public sealed class DeleteBasketCommandHandler : IRequestHandler<DeleteBasketCommand, bool>
    {
        private readonly IBasketRepository _repo;
        public DeleteBasketCommandHandler(IBasketRepository repo) => _repo = repo;

        public async Task<bool> Handle(DeleteBasketCommand request, CancellationToken cancellationToken)
        {
            return await _repo.DeleteBasketAsync(request.Id);
        }
    }
}
