using BuildingBlocks.Results;
using CustomerService.Domain.Errors;

namespace CustomerService.Domain.ValueObjects
{
    public class Password
    {
        private Password(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public static Result<Password> Create(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return Result.Failure<Password>(CustomerError.PasswordEmpty);

            if (password.Length < 8)
                return Result.Failure<Password>(CustomerError.PasswordWeak);

            return Result.Success(new Password(password));
        }

        public override string ToString()
        {
            return Value;
        }
    }
}