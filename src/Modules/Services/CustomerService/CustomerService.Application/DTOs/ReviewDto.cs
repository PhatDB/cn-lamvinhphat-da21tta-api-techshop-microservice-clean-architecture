using CustomerService.Domain.ValueObjects;

namespace CustomerService.Application.DTOs
{
    public record ReviewDto
    {
        public int CustomerId { get; init; }
        public int ProductId { get; init; }
        public string ProductName { get; init; }
        public string ImageUrl { get; init; }
        public string CustomerName { get; init; }
        public Email Email { get; init; }
        public string Comment { get; init; }
        public byte Rating { get; init; }
        public bool IsVerified { get; init; }
    }
}