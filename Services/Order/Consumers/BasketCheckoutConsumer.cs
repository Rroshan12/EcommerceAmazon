using MassTransit;
using EventBus.Messages.Events;
using Order.Dtos;
using Order.Entities;
using Order.Repositories;

namespace Order.Consumers
{
    public class BasketCheckoutConsumer : IConsumer<BasketCheckoutEvent>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<BasketCheckoutConsumer> _logger;

        public BasketCheckoutConsumer(IOrderRepository orderRepository, ILogger<BasketCheckoutConsumer> logger)
        {
            _orderRepository = orderRepository;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
        {
            try
            {
                _logger.LogInformation("Processing BasketCheckoutEvent for UserId: {UserId}", context.Message.UserId);

                // Create order from checkout event
                var orderEntity = new OrderEntity
                {
                    UserId = context.Message.UserId,
                    FirstName = context.Message.FirstName,
                    LastName = context.Message.LastName,
                    EmailAddress = context.Message.EmailAddress,
                    AddressLine = context.Message.AddressLine,
                    Country = context.Message.Country,
                    State = context.Message.State,
                    ZipCode = context.Message.ZipCode,
                    TotalPrice = context.Message.TotalPrice,
                    OrderItems = context.Message.BasketItems.Select(item => new OrderItem
                    {
                        ProductId = item.ProductId,
                        ProductName = item.ProductName,
                        UnitPrice = item.UnitPrice,
                        Quantity = item.Quantity,
                        DiscountAmount = item.DiscountAmount
                    }).ToList()
                };

                // Save order to database
                var createdOrder = await _orderRepository.CreateOrderAsync(orderEntity);

                _logger.LogInformation("Order created successfully. OrderId: {OrderId}, UserId: {UserId}", createdOrder.Id, context.Message.UserId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing BasketCheckoutEvent for UserId: {UserId}", context.Message.UserId);
                throw;
            }
        }
    }
}
