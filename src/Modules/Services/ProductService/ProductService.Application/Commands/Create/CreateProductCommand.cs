using BuildingBlocks.CQRS;
using ProductService.Application.DTOs;

namespace ProductService.Application.Commands.Create
{
    public record CreateProductCommand : ICommand<int>
    {
        public CreateProductCommand(string name, string sku, decimal price, int categoryId, string? description, decimal? discountPrice)
        {
            Name = name;
            SKU = sku;
            Price = price;
            CategoryId = categoryId;
            Description = description;
            DiscountPrice = discountPrice;
        }

        public string Name { get; set; }
        public string SKU { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public string? Description { get; set; }
        public decimal? DiscountPrice { get; set; }
        public List<ProductImageDTO> Images { get; set; } = new();
        public List<string> Colors { get; set; } = new();
    }
}