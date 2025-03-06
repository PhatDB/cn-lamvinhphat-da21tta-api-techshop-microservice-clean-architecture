using BuildingBlocks.Abstractions.Entities;
using BuildingBlocks.Results;
using UserService.Domain.ValueObjects;

namespace UserService.Domain.Entities
{
    public class UserAddress : Entity
    {
        private UserAddress(
            int userId, string addressLine, PhoneNumber phoneNumber, string province,
            string district)
        {
            UserId = userId;
            AddressLine = addressLine;
            PhoneNumber = phoneNumber;
            Province = province;
            District = district;
        }

        private UserAddress()
        {
        }

        public int UserId { get; private set; }
        public string AddressLine { get; private set; }
        public PhoneNumber PhoneNumber { get; private set; }
        public string Province { get; private set; }
        public string District { get; private set; }

        public User User { get; }

        public static Result<UserAddress> Create(
            int userId, string addressLine, string phoneNumber, string province,
            string district)
        {
            Result<PhoneNumber> phoneResult = PhoneNumber.Create(phoneNumber);
            if (phoneResult.IsFailure)
                return Result.Failure<UserAddress>(phoneResult.Error);

            return Result.Success(new UserAddress(userId, addressLine, phoneResult.Value,
                province, district));
        }

        public Result UpdateAddress(
            string? addressLine, string? phoneNumber, string? province, string? district)
        {
            if (!string.IsNullOrWhiteSpace(addressLine))
                AddressLine = addressLine;

            if (!string.IsNullOrWhiteSpace(phoneNumber))
            {
                Result<PhoneNumber> phoneResult = PhoneNumber.Create(phoneNumber);
                if (phoneResult.IsFailure)
                    return Result.Failure(phoneResult.Error);

                PhoneNumber = phoneResult.Value;
            }

            Province = province ?? Province;
            District = district ?? District;

            return Result.Success();
        }
    }
}