namespace BuildingBlocks.Contracts.Customers
{
    public class ProductReviewDto
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public List<ReviewDto> Reviews { get; set; }
    }
}