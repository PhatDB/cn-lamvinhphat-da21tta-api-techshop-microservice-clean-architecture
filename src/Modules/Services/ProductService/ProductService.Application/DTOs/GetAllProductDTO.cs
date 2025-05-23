using ProductService.Domain.Entities;

namespace ProductService.Application.DTOs
{
    public record GetAllProductDTO
    {
        public int Id { get; init; }
        public string ProductName { get; init; }
        public decimal Price { get; init; }
        public decimal? Discount { get; init; }
        public bool IsActive { get; init; }
        public bool IsFeatured { get; init; }
        public int Stock { get; init; }
        public int SoldQuantity { get; init; }
        public string Specs { get; init; }
        public string ImageUrl { get; init; }
        public byte Rating { get; set; }
        public List<ProductSpec> ProductSpecs { get; init; }
    }
}