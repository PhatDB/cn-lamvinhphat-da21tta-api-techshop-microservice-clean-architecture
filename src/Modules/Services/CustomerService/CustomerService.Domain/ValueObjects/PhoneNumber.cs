using System.Text.RegularExpressions;
using BuildingBlocks.Results;
using CustomerService.Domain.Errors;

namespace CustomerService.Domain.ValueObjects
{
    public class PhoneNumber
    {
        private static readonly Regex PhoneRegex = new(@"^\+?[0-9]{10,15}$", RegexOptions.Compiled);

        private PhoneNumber(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public static Result<PhoneNumber> Create(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return Result.Failure<PhoneNumber>(CustomerError.PhoneNumberEmpty);

            if (!PhoneRegex.IsMatch(phoneNumber))
                return Result.Failure<PhoneNumber>(CustomerError.PhoneNumberInvalid);

            return Result.Success(new PhoneNumber(phoneNumber));
        }

        public override string ToString()
        {
            return Value;
        }
    }
}