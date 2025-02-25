namespace ProductService.Application.DTOs
{
    public record ProductImageDTO
    {
        public string ImageContent { get; init; }
        public string? Title { get; init; }
        public int Position { get; init; }
    }
}