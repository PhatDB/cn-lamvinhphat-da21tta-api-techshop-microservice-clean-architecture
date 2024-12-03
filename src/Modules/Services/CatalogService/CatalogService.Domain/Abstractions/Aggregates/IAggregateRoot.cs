using CatalogService.Domain.Abstractions.Entities;

namespace CatalogService.Domain.Abstractions.Aggregates
{
    public interface IAggregateRoot<T> : IEntity<T>
    {
        IReadOnlyList<IDomainEvent> DomainEvents { get; }
        IDomainEvent[] ClearDomainEvents();
    }
}