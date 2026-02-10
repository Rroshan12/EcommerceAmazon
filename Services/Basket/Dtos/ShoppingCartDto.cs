using System.Collections.Generic;

namespace Basket.Dtos
{
    public class ShoppingCartItemDto
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public string ImageUrl { get; set; }
        public int DiscountAmount { get; set; } // discount percentage or amount
    }

    public class ShoppingCartDto
    {
        public string Id { get; set; }
        public List<ShoppingCartItemDto> Items { get; set; } = new();
        public decimal TotalPrice { get; set; }
    }

    public class BasketCheckoutDto
    {
        public string UserId { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public decimal Total { get; set; }
    }
}