using Basket.Entities;

namespace Basket.Repositories
{
    public interface IBasketRepository
    {
        Task<ShoppingCart> GetBasketAsync(string id);
        Task<ShoppingCart> UpdateBasketAsync(ShoppingCart basket);
        Task<bool> DeleteBasketAsync(string id);
    }
}
