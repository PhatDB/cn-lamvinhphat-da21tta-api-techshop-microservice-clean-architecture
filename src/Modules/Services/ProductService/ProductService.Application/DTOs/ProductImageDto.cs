namespace ProductService.Application.DTOs
{
    public record ProductImageDto(string ImageContent, bool IsMain, int SortOrder);
}