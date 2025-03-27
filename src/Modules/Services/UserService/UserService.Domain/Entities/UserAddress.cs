using BuildingBlocks.Abstractions.Entities;
using BuildingBlocks.Results;
using UserService.Domain.ValueObjects;

namespace UserService.Domain.Entities
{
    public class UserAddress : Entity
    {
        private UserAddress(int userId, string street, string city, string district, string ward, string zipCode, PhoneNumber phoneNumber)
        {
            UserId = userId;
            Street = street;
            City = city;
            District = district;
            Ward = ward;
            ZipCode = zipCode;
            PhoneNumber = phoneNumber;
        }

        private UserAddress()
        {
        }

        public int UserId { get; private set; }
        public string Street { get; private set; }
        public string City { get; private set; }
        public string District { get; private set; }
        public string Ward { get; private set; }
        public string ZipCode { get; private set; }
        public PhoneNumber PhoneNumber { get; private set; }


        public static Result<UserAddress> Create(int userId, string street, string city, string? district, string? ward, string zipCode, string phoneNumber)
        {
            Result<PhoneNumber> phoneResult = PhoneNumber.Create(phoneNumber);
            if (phoneResult.IsFailure)
                return Result.Failure<UserAddress>(phoneResult.Error);

            return Result.Success(new UserAddress(userId, street, city, district, ward, zipCode, phoneResult.Value));
        }

        public Result UpdateAddress(string? street, string? city, string? district, string? ward, string? zipCode, string? phoneNumber)
        {
            if (!string.IsNullOrWhiteSpace(phoneNumber))
            {
                Result<PhoneNumber> phoneResult = PhoneNumber.Create(phoneNumber);
                if (phoneResult.IsFailure)
                    return Result.Failure(phoneResult.Error);

                PhoneNumber = phoneResult.Value;
            }

            Street = street ?? Street;
            City = city ?? City;
            District = district ?? District;
            Ward = ward ?? Ward;
            ZipCode = zipCode ?? ZipCode;

            return Result.Success();
        }
    }
}