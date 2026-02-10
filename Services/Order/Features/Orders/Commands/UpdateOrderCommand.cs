using MediatR;
using Order.Dtos;
using Order.Entities;
using Order.Repositories;

namespace Order.Features.Orders.Commands
{
    public sealed class UpdateOrderCommand : IRequest<OrderDto>
    {
        public UpdateOrderRequest Request { get; }
        public UpdateOrderCommand(UpdateOrderRequest request) => Request = request;
    }

    public sealed class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, OrderDto>
    {
        private readonly IOrderRepository _repo;
        public UpdateOrderCommandHandler(IOrderRepository repo) => _repo = repo;

        public async Task<OrderDto> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = new OrderEntity
            {
                Id = request.Request.Id,
                FirstName = request.Request.FirstName,
                LastName = request.Request.LastName,
                EmailAddress = request.Request.EmailAddress,
                AddressLine = request.Request.AddressLine,
                Country = request.Request.Country,
                State = request.Request.State,
                ZipCode = request.Request.ZipCode
            };

            return await _repo.UpdateOrderAsync(order);
        }
    }
}

