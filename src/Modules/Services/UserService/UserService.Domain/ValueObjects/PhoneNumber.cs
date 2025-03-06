using System.Text.RegularExpressions;
using BuildingBlocks.Results;
using UserService.Domain.Errors;

namespace UserService.Domain.ValueObjects
{
    public class PhoneNumber
    {
        private static readonly Regex PhoneRegex =
            new(@"^\+?[0-9]{10,15}$", RegexOptions.Compiled);

        private PhoneNumber(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public static Result<PhoneNumber> Create(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return Result.Failure<PhoneNumber>(UserError.PhoneNumberEmpty);

            if (!PhoneRegex.IsMatch(phoneNumber))
                return Result.Failure<PhoneNumber>(UserError.PhoneNumberInvalid);

            return Result.Success(new PhoneNumber(phoneNumber));
        }

        public override string ToString()
        {
            return Value;
        }
    }
}