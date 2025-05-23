namespace BuildingBlocks.Contracts.Customers
{
    public class ReviewDto
    {
        public int ReviewId { get; set; }
        public byte? Rating { get; set; }
        public string? Comment { get; set; }
        public bool? IsVerified { get; set; }
        public DateTime Created_at { get; set; }
    }
}