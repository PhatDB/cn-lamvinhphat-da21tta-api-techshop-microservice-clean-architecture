namespace ProductService.Application.DTOs
{
    public record ProductDetailDTO(
        int Id,
        string Name,
        string Sku,
        decimal Price,
        decimal? DiscountPrice,
        int SoldQuantity,
        bool IsActive,
        int CategoryId,
        int StockQuantity,
        List<string> Images)
    {
        public ProductDetailDTO() : this(0, string.Empty, string.Empty, 0, null, 0, true,
            0, 0, new List<string>())
        {
        }
    }
}