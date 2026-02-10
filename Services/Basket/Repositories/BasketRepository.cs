using Basket.Entities;
using StackExchange.Redis;
using Newtonsoft.Json;

namespace Basket.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _db;

        public BasketRepository(IConnectionMultiplexer redis)
        {
            _db = redis.GetDatabase();
        }

        public async Task<ShoppingCart> GetBasketAsync(string id)
        {
            var data = await _db.StringGetAsync(id);
            if (data.IsNullOrEmpty) return null;
            return JsonConvert.DeserializeObject<ShoppingCart>(data!);
        }

        public async Task<ShoppingCart> UpdateBasketAsync(ShoppingCart basket)
        {
            var created = await _db.StringSetAsync(basket.Id, JsonConvert.SerializeObject(basket));
            if (!created) return null;
            return await GetBasketAsync(basket.Id);
        }

        public async Task<bool> DeleteBasketAsync(string id)
        {
            return await _db.KeyDeleteAsync(id);
        }
    }
}
