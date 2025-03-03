namespace ProductService.Application.DTOs
{
    public class GetAllProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Sku { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public decimal? DiscountPrice { get; set; }
        public int SoldQuantity { get; set; }
        public bool IsActive { get; set; }
        public int CategoryId { get; set; }
        public string? FirstImageUrl { get; set; }
        public List<ColorDTO> Colors { get; set; } = new();
    }
}