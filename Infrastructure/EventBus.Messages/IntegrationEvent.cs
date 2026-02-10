namespace EventBus.Messages
{
    public class IntegrationEvent
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTime CreatedTime { get; set; } = DateTime.UtcNow;
    }
}
