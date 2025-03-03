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
        string? FirstImageUrl,
        List<ColorDTO> Colors)
    {
        public GetAllProductDTO() : this(0, string.Empty, string.Empty, null, 0, null, 0,
            true, 0, null, new List<ColorDTO>())
        {
        }
    }
}