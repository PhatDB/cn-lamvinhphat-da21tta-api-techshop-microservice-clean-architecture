namespace ProductService.Application.DTOs
{
    public record GetAllProductDTO(
        int Id,
        string Name,
        string Sku,
        string? Description,
        decimal Price,
        decimal? DiscountPrice,
        int SoldQuantity,
        bool IsActive,
        int CategoryId,
        int StockQuantity,
        string? FirstImageUrl)
    {
        public GetAllProductDTO() : this(0, string.Empty, string.Empty, null, 0, null, 0,
            true, 0, 0, null)
        {
        }
    }
}