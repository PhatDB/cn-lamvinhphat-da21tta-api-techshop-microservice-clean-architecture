using BuildingBlocks.Abstractions.Entities;
using BuildingBlocks.Results;

namespace CustomerService.Domain.Entities
{
    public class Address : Entity
    {
        private Address(int customerId, string? street, string? hamlet, string? ward, string? district, string? city)
        {
            CustomerId = customerId;
            Street = street;
            Hamlet = hamlet;
            Ward = ward;
            District = district;
            City = city;
            CreatedAt = DateTime.UtcNow;
        }

        public int CustomerId { get; private set; }
        public string? Street { get; private set; }
        public string? Hamlet { get; private set; }
        public string? Ward { get; private set; }
        public string? District { get; private set; }
        public string? City { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public static Result<Address> Create(
            int customerId, string? street, string? hamlet, string? ward, string? district, string? city)
        {
            return Result.Success(new Address(customerId, street, hamlet, ward, district, city));
        }
    }
}