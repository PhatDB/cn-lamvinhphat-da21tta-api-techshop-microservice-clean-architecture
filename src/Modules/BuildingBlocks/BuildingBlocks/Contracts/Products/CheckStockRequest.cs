namespace BuildingBlocks.Contracts.Products
{
    public record CheckStockRequest(int ProductId, int QuantityRequested);
}