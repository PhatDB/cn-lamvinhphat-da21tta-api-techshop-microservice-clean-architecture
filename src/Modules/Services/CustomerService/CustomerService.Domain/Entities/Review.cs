using BuildingBlocks.Abstractions.Entities;
using BuildingBlocks.Results;

namespace CustomerService.Domain.Entities
{
    public class Review : Entity
    {
        public Review(int productId, int customerId, byte? rating, string? comment)
        {
            ProductId = productId;
            CustomerId = customerId;
            Rating = rating;
            Comment = comment;
            IsVerified = true;
            CreatedAt = DateTime.UtcNow;
        }

        public int ProductId { get; private set; }
        public int CustomerId { get; private set; }
        public byte? Rating { get; private set; }
        public string? Comment { get; private set; }
        public bool? IsVerified { get; private set; }
        public DateTime? CreatedAt { get; private set; }

        public static Result<Review> Create(int productId, int customerId, byte? rating, string? comment)
        {
            return new Review(productId, customerId, rating, comment);
        }
    }
}