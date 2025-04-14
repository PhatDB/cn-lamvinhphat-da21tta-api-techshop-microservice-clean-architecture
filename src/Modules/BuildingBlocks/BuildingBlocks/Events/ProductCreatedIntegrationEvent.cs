namespace BuildingBlocks.Events
{
    public record ProductCreatedIntegrationEvent(int ProductId, string ProductName, decimal Price) : IntegrationEvent;
}