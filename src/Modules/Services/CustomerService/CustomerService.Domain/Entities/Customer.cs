using System.Security.Cryptography;
using BuildingBlocks.Abstractions.Aggregates;
using BuildingBlocks.Abstractions.Entities;
using BuildingBlocks.Results;
using CustomerService.Domain.Errors;
using CustomerService.Domain.ValueObjects;

namespace CustomerService.Domain.Entities
{
    public class Customer : Entity, IAggregateRoot
    {
        private readonly List<Address> _addresses;
        private readonly List<Review> _reviews;

        public Customer(string customerName, Email email, PhoneNumber? phone, Password? password)
        {
            CustomerName = customerName;
            Email = email;
            Phone = phone;
            Password = password;
            Status = false;
            Role = "Customer";
            Otp = GenerateOTP();
            OtpExpired = DateTime.UtcNow.AddMinutes(5);
            _addresses = new List<Address>();
            _reviews = new List<Review>();
        }

        public string CustomerName { get; private set; }
        public Email Email { get; private set; }
        public PhoneNumber? Phone { get; private set; }
        public Password? Password { get; private set; }
        public bool? Status { get; private set; }
        public string? Otp { get; private set; }
        public string Role { get; private set; }
        public DateTime? OtpExpired { get; private set; }
        public IReadOnlyCollection<Address> Addresses => _addresses.AsReadOnly();
        public IReadOnlyCollection<Review> Reviews => _reviews.AsReadOnly();

        public static Result<Customer> Create(string customerName, string email, string? phone, string? password)
        {
            if (string.IsNullOrWhiteSpace(customerName))
                return Result.Failure<Customer>(CustomerError.CustomerNameEmpty);

            Result<Email> emailResult = Email.Create(email);
            if (emailResult.IsFailure)
                return Result.Failure<Customer>(emailResult.Error);

            Result<PhoneNumber>? phoneResult = null;
            PhoneNumber? phoneValue = null;

            if (!string.IsNullOrWhiteSpace(phone))
            {
                phoneResult = PhoneNumber.Create(phone);
                if (phoneResult.IsFailure)
                    return Result.Failure<Customer>(phoneResult.Error);

                phoneValue = phoneResult.Value;
            }

            Result<Password>? passwordResult = null;
            Password? passwordValue = null;

            if (!string.IsNullOrWhiteSpace(password))
            {
                passwordResult = Password.Create(password);
                if (passwordResult.IsFailure)
                    return Result.Failure<Customer>(passwordResult.Error);

                passwordValue = passwordResult.Value;
            }

            return Result.Success(new Customer(customerName, emailResult.Value, phoneValue, passwordValue));
        }

        public Result UpdatePassword(string newPassword)
        {
            Result<Password> passwordResult = Password.Create(newPassword);
            if (passwordResult.IsFailure)
                return Result.Failure<Customer>(passwordResult.Error);

            Password = passwordResult.Value;
            return Result.Success();
        }

        public Result UpdateCustomerInfo(string? customerName, string? email, string? phoneNumber)
        {
            bool updated = false;

            if (!string.IsNullOrWhiteSpace(customerName) && customerName.Trim() != CustomerName)
            {
                CustomerName = customerName.Trim();
                updated = true;
            }

            if (!string.IsNullOrWhiteSpace(email) && email != Email.Value)
            {
                Result<Email> emailResult = Email.Create(email);
                if (emailResult.IsFailure)
                    return Result.Failure<Customer>(emailResult.Error);

                Email = emailResult.Value;
                updated = true;
            }

            if (!string.IsNullOrWhiteSpace(phoneNumber) && phoneNumber != Phone.Value)
            {
                Result<PhoneNumber> phoneResult = PhoneNumber.Create(phoneNumber);
                if (phoneResult.IsFailure)
                    return Result.Failure<Customer>(phoneResult.Error);

                Phone = phoneResult.Value;
                updated = true;
            }

            return updated ? Result.Success() : Result.Failure<Customer>(CustomerError.NoChangeDetected);
        }

        public Result AddAddress(string? street, string? hamlet, string? ward, string? district, string? city)
        {
            Result<Address> addressResult = Address.Create(Id, street, hamlet, ward, district, city);
            if (addressResult.IsFailure)
                return Result.Failure(addressResult.Error);

            if (_addresses.Any(a => a.Equals(addressResult.Value)))
                return Result.Failure(CustomerError.AddressAlreadyExists);

            _addresses.Add(addressResult.Value);
            return Result.Success();
        }

        public Result RemoveAddress(int addressId)
        {
            Address? address = _addresses.FirstOrDefault(a => a.Id == addressId);
            if (address is null)
                return Result.Failure(CustomerError.AddressNotFound);

            _addresses.Remove(address);
            return Result.Success();
        }

        public Result AddReview(Review review)
        {
            _reviews.Add(review);
            return Result.Success();
        }

        public void ActivateCustomer()
        {
            Status = true;
        }

        public void UpdateOTP()
        {
            Otp = GenerateOTP();
            OtpExpired = DateTime.UtcNow.AddMinutes(5);
        }

        public bool ValidateOTP(string otp)
        {
            if (Otp != otp || OtpExpired is null || OtpExpired < DateTime.UtcNow)
                return false;

            Status = true;
            return true;
        }

        private string GenerateOTP(int length = 6)
        {
            const string allowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            char[] otp = new char[length];

            using RandomNumberGenerator rng = RandomNumberGenerator.Create();
            byte[] buffer = new byte[sizeof(uint)];

            for (int i = 0; i < length; i++)
            {
                rng.GetBytes(buffer);
                uint num = BitConverter.ToUInt32(buffer, 0);
                otp[i] = allowedChars[(int)(num % (uint)allowedChars.Length)];
            }

            return new string(otp);
        }
    }
}