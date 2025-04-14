using BuildingBlocks.Events;
using MassTransit;
using MediatR;
using ProductService.Domain.Events;

namespace ProductService.Infracstructure.EventHandler
{
    public class ProductCreatedDomainEventHandler : INotificationHandler<ProductCreatedDomainEvent>
    {
        private readonly IPublishEndpoint _publish;

        public ProductCreatedDomainEventHandler(IPublishEndpoint publish)
        {
            _publish = publish;
        }

        public async Task Handle(ProductCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            ProductCreatedIntegrationEvent integrationEvent = new(notification.ProductId, notification.ProductName,
                notification.Price);

            await _publish.Publish(integrationEvent, cancellationToken);
        }
    }
}