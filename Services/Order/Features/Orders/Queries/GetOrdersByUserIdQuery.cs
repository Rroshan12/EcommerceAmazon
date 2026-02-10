using MediatR;
using Order.Dtos;
using Order.Repositories;

namespace Order.Features.Orders.Queries
{
    public sealed class GetOrdersByUserIdQuery : IRequest<IEnumerable<OrderDto>>
    {
        public string UserId { get; }
        public GetOrdersByUserIdQuery(string userId) => UserId = userId;
    }

    public sealed class GetOrdersByUserIdQueryHandler : IRequestHandler<GetOrdersByUserIdQuery, IEnumerable<OrderDto>>
    {
        private readonly IOrderRepository _repo;
        public GetOrdersByUserIdQueryHandler(IOrderRepository repo) => _repo = repo;

        public async Task<IEnumerable<OrderDto>> Handle(GetOrdersByUserIdQuery request, CancellationToken cancellationToken)
        {
            return await _repo.GetOrdersByUserIdAsync(request.UserId);
        }
    }
}
