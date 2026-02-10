using MediatR;
using Order.Dtos;
using Order.Entities;
using Order.Repositories;

namespace Order.Features.Orders.Commands
{
    public sealed class CreateOrderCommand : IRequest<OrderDto>
    {
        public CreateOrderRequest Request { get; }
        public CreateOrderCommand(CreateOrderRequest request) => Request = request;
    }

    public sealed class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, OrderDto>
    {
        private readonly IOrderRepository _repo;
        public CreateOrderCommandHandler(IOrderRepository repo) => _repo = repo;

        public async Task<OrderDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = new OrderEntity
            {
                UserId = request.Request.UserId,
                FirstName = request.Request.FirstName,
                LastName = request.Request.LastName,
                EmailAddress = request.Request.EmailAddress,
                AddressLine = request.Request.AddressLine,
                Country = request.Request.Country,
                State = request.Request.State,
                ZipCode = request.Request.ZipCode,
                TotalPrice = request.Request.OrderItems.Sum(oi => (oi.UnitPrice * oi.Quantity) - oi.DiscountAmount),
                OrderItems = request.Request.OrderItems.Select(oi => new OrderItem
                {
                    ProductId = oi.ProductId,
                    ProductName = oi.ProductName,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice,
                    DiscountAmount = oi.DiscountAmount
                }).ToList()
            };

            return await _repo.CreateOrderAsync(order);
        }
    }
}

