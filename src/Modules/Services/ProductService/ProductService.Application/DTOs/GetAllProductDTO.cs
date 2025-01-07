namespace ProductService.Application.DTOs
{
    public class GetAllProductDTO
    {
        public int Id { get; set; }
        public string? ProductName { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public decimal? DiscountPrice { get; set; }
        public bool IsActive { get; set; }
        public string? FirstImageUrl { get; set; }
    }
}