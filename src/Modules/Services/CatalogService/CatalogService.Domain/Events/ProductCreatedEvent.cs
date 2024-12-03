using CatalogService.Domain.Abstractions;

namespace CatalogService.Domain.Events
{
    public class ProductCreatedEvent : IDomainEvent
    {
        public ProductCreatedEvent(Guid productId, string sku, decimal price)
        {
            ProductId = productId;
            SKU = sku;
            Price = price;
        }

        public Guid ProductId { get; }
        public string SKU { get; }
        public decimal Price { get; }

        public Guid EventId => Guid.NewGuid();
        public DateTime OccurredOn => DateTime.UtcNow;
        public string EventType => GetType().AssemblyQualifiedName;
    }
}