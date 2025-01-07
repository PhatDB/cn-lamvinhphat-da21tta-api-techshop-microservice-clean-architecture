namespace ProductService.Application.DTOs
{
    public record ProductImageDTO
    {
        public string ImageContent { get; init; }
        public string? AltText { get; init; }
    }
}