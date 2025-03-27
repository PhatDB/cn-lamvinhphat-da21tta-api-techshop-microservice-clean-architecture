using BuildingBlocks.Abstractions.Aggregates;
using BuildingBlocks.Abstractions.Entities;
using BuildingBlocks.Results;
using UserService.Domain.Errors;
using UserService.Domain.ValueObjects;

namespace UserService.Domain.Entities
{
    public class User : Entity, IAggregateRoot
    {
        private readonly List<UserAddress> _userAddresses;

        private User(string username, Email email, Password password)
        {
            Username = username;
            Email = email;
            Password = password;
            Role = "user";
            CreatedAt = DateTime.UtcNow;
            IsActive = true;
            LastLogin = null;
            _userAddresses = new List<UserAddress>();
        }

        private User()
        {
            _userAddresses = new List<UserAddress>();
        }

        public string Username { get; private set; }
        public Email Email { get; }
        public Password Password { get; private set; }
        public string Role { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? LastLogin { get; private set; }
        public bool IsActive { get; private set; }

        public IReadOnlyCollection<UserAddress> UserAddresses => _userAddresses.AsReadOnly();

        public static Result<User> Create(string username, string email, string password)
        {
            if (string.IsNullOrWhiteSpace(username))
                return Result.Failure<User>(UserError.UsernameEmpty);

            Result<Email> emailResult = Email.Create(email);
            if (emailResult.IsFailure)
                return Result.Failure<User>(emailResult.Error);

            Result<Password> passwordResult = Password.Create(password);
            if (passwordResult.IsFailure)
                return Result.Failure<User>(passwordResult.Error);

            return Result.Success(new User(username, emailResult.Value, passwordResult.Value));
        }

        public Result UpdateUserAndAddress(string? userName, string? street, string? city, string? district, string? ward, string? zipCode, string? phoneNumber)
        {
            Username = userName ?? Username;

            UserAddress? address = _userAddresses.FirstOrDefault();

            if (address == null)
            {
                Result<UserAddress> newAddressResult = UserAddress.Create(Id, street, city, district, ward, zipCode, phoneNumber);
                if (newAddressResult.IsFailure)
                    return Result.Failure(newAddressResult.Error);

                _userAddresses.Add(newAddressResult.Value);
            }
            else
            {
                Result updateAddressResult = address.UpdateAddress(street, city, district, ward, zipCode, phoneNumber);
                if (updateAddressResult.IsFailure)
                    return Result.Failure(updateAddressResult.Error);
            }

            return Result.Success();
        }

        public Result UpdatePassword(string newPassword)
        {
            Result<Password> passwordResult = Password.Create(newPassword);
            if (passwordResult.IsFailure)
                return Result.Failure<User>(passwordResult.Error);

            Password = passwordResult.Value;
            return Result.Success();
        }

        public void UpdateLastLogin()
        {
            LastLogin = DateTime.UtcNow;
        }
    }
}