namespace BuildingBlocks.Contracts.Products
{
    public record CheckStockResult(int ProductId, int QuantityRequested, int QuantityAvailable, bool IsAvailable);
}