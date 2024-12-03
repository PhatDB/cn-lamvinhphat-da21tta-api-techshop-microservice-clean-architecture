using CatalogService.Domain.Abstractions;

namespace CatalogService.Domain.Events
{
    public class CategoryUpdatedEvent : IDomainEvent
    {
        public CategoryUpdatedEvent(Guid categoryId, string name, string? description)
        {
            CategoryId = categoryId;
            Name = name;
            Description = description;
        }

        public Guid CategoryId { get; }
        public string Name { get; }
        public string? Description { get; }

        public Guid EventId => Guid.NewGuid();
        public DateTime OccurredOn => DateTime.UtcNow;
        public string EventType => GetType().AssemblyQualifiedName;
    }
}