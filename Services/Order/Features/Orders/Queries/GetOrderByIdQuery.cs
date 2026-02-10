using MediatR;
using Order.Dtos;
using Order.Repositories;

namespace Order.Features.Orders.Queries
{
    public sealed class GetOrderByIdQuery : IRequest<OrderDto>
    {
        public int Id { get; }
        public GetOrderByIdQuery(int id) => Id = id;
    }

    public sealed class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, OrderDto>
    {
        private readonly IOrderRepository _repo;
        public GetOrderByIdQueryHandler(IOrderRepository repo) => _repo = repo;

        public async Task<OrderDto> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            return await _repo.GetOrderByIdAsync(request.Id);
        }
    }
}
