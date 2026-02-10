using Order.Entities;
using Order.Dtos;
using System.Collections.Generic;

namespace Order.Repositories
{
    public interface IOrderRepository
    {
        Task<OrderDto> GetOrderByIdAsync(int id);
        Task<IEnumerable<OrderDto>> GetOrdersByUserIdAsync(string userId);
        Task<OrderDto> CreateOrderAsync(OrderEntity order);
        Task<OrderDto> UpdateOrderAsync(OrderEntity order);
        Task<bool> DeleteOrderAsync(int id);
    }
}

