namespace ProductService.Application.DTOs
{
    public record ProductDetailDTO
    {
        public int ProductId { get; init; }
        public string ProductName { get; init; }
        public string Description { get; init; }
        public decimal Price { get; init; }
        public decimal Discount { get; init; }
        public int Stock { get; init; }
        public int SoldQuantity { get; init; }
        public bool IsActive { get; init; }
        public bool IsFeatured { get; init; }
        public DateTime CreatedAt { get; init; }
        public string Specs { get; init; }
        public List<ImageDto> Images { get; init; }
    }
}