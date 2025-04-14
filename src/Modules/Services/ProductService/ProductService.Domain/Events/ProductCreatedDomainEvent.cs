using MediatR;

namespace ProductService.Domain.Events
{
    public record ProductCreatedDomainEvent(int ProductId, string ProductName, decimal Price) : INotification;
}