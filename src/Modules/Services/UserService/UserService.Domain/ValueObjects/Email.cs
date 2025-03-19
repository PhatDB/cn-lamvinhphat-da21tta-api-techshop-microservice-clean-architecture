using System.Text.RegularExpressions;
using BuildingBlocks.Results;
using UserService.Domain.Errors;

namespace UserService.Domain.ValueObjects
{
    public class Email
    {
        private static readonly Regex EmailRegex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

        public Email(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public static Result<Email> Create(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return Result.Failure<Email>(UserError.EmailEmpty);

            if (!EmailRegex.IsMatch(email))
                return Result.Failure<Email>(UserError.EmailInvalidFormat);

            return Result.Success(new Email(email));
        }

        public override string ToString()
        {
            return Value;
        }
    }
}