using BuildingBlocks.Results;
using UserService.Domain.Errors;

namespace UserService.Domain.ValueObjects
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
                return Result.Failure<Password>(UserError.PasswordEmpty);

            if (password.Length < 8 || !password.Any(char.IsDigit) ||
                !password.Any(char.IsPunctuation))
                return Result.Failure<Password>(UserError.PasswordWeak);

            return Result.Success(new Password(password));
        }

        public override string ToString()
        {
            return Value;
        }
    }
}