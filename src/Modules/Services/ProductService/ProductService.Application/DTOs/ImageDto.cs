namespace ProductService.Application.DTOs
{
    public record ImageDto
    {
        public int ImageId { get; init; }
        public string ImageUrl { get; init; }
        public bool IsMain { get; init; }
        public int SortOrder { get; init; }
    }
}