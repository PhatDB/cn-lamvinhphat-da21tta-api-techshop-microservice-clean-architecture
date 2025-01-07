using BuildingBlocks.CQRS;

namespace ProductService.Application.Commands.Update
{
    public record UpdateProductCommand : ICommand
    {
        public UpdateProductCommand(
            int productId, string? productName, decimal? price, string? description = null, decimal? discountPrice = null, string? sku = null, string? brand = null,
            string? model = null, int? stockStatus = null)
        {
            ProductId = productId;
            ProductName = productName;
            Price = price;
            Description = description;
            DiscountPrice = discountPrice;
            SKU = sku;
            Brand = brand;
            Model = model;
            StockStatus = stockStatus;
        }

        public int ProductId { get; init; }
        public string? ProductName { get; init; }
        public string? Description { get; init; }
        public decimal? Price { get; init; }
        public decimal? DiscountPrice { get; init; }
        public string? SKU { get; init; }
        public string? Brand { get; init; }
        public string? Model { get; init; }
        public int? StockStatus { get; init; }
    }
}