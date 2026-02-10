namespace EventBus.Messages.Events
{
    public class BasketCheckoutEvent : IntegrationEvent
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string AddressLine { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public decimal TotalPrice { get; set; }
        public List<BasketCheckoutItem> BasketItems { get; set; } = new();
    }

    public class BasketCheckoutItem
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public int DiscountAmount { get; set; }
    }
}
