using System.Collections.Generic;

namespace Basket.Entities
{
    public class ShoppingCart
    {
        public string Id { get; set; } // user id or cart id
        public List<ShoppingCartItem> Items { get; set; } = new();

        public decimal TotalPrice => Items?.Sum(i => i.UnitPrice * i.Quantity) ?? 0m;
    }
}