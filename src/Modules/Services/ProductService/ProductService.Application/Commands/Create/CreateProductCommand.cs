using BuildingBlocks.CQRS;
using ProductService.Application.DTOs;

namespace ProductService.Application.Commands.Create
{
    public record CreateProductCommand : ICommand<int>
    {
        public CreateProductCommand(
            string productName, decimal price, string? description = null, decimal? discountPrice = null, string? sku = null, string? brand = null, string? model = null,
            int? stockStatus = null, List<ProductImageDTO>? productImages = null)
        {
            ProductName = productName;
            Price = price;
            Description = description;
            DiscountPrice = discountPrice;
            SKU = sku;
            Brand = brand;
            Model = model;
            StockStatus = stockStatus;
            ProductImages = productImages ?? new List<ProductImageDTO>();
        }

        public string ProductName { get; init; }
        public string? Description { get; init; }
        public decimal Price { get; init; }
        public decimal? DiscountPrice { get; init; }
        public string? SKU { get; init; }
        public string? Brand { get; init; }
        public string? Model { get; init; }
        public int? StockStatus { get; init; }
        public List<ProductImageDTO> ProductImages { get; init; }
    }
}