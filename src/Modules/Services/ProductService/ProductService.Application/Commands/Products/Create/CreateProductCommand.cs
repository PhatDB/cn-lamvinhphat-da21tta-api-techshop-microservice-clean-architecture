using BuildingBlocks.CQRS;
using ProductService.Application.DTOs;

namespace ProductService.Application.Commands.Products.Create
{
    public record CreateProductCommand : ICommand<int>
    {
        public CreateProductCommand(
            string name, string sku, decimal price, int categoryId,
            List<ProductImageDTO> images, List<ColorDTO> colors,
            string? description = null, decimal? discountPrice = null)
        {
            Name = name;
            SKU = sku;
            Price = price;
            CategoryId = categoryId;
            Description = description;
            DiscountPrice = discountPrice;
            Images = images;
            Colors = colors;
        }

        public string Name { get; set; }
        public string SKU { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public string? Description { get; set; }
        public decimal? DiscountPrice { get; set; }
        public List<ProductImageDTO> Images { get; set; }
        public List<ColorDTO> Colors { get; set; }
    }
}