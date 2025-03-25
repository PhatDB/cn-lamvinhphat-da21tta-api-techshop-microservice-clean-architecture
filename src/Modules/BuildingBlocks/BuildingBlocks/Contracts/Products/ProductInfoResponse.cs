namespace BuildingBlocks.Contracts.Products
{
    public record ProductInfoResponse(int ProductId, string Name, decimal Price, string ImageUrl, string Description, int StockQuantity);
}