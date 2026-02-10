using System.Collections.Generic;

namespace Order.Entities
{
    public class OrderItem : BaseEntity
    {
        public int OrderId { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public int DiscountAmount { get; set; }

        public virtual OrderEntity Order { get; set; }
    }
}

