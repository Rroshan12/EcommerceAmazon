using MediatR;
using Order.Repositories;

namespace Order.Features.Orders.Commands
{
    public sealed class DeleteOrderCommand : IRequest<bool>
    {
        public int Id { get; }
        public DeleteOrderCommand(int id) => Id = id;
    }

    public sealed class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, bool>
    {
        private readonly IOrderRepository _repo;
        public DeleteOrderCommandHandler(IOrderRepository repo) => _repo = repo;

        public async Task<bool> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            return await _repo.DeleteOrderAsync(request.Id);
        }
    }
}
