namespace BuildingBlocks.Contracts.Products
{
    public record ProductInfoResponse(
        int ProductId,
        string ProductName,
        string ImageUrl,
        decimal Price,
        decimal Discount,
        int Stock);
}