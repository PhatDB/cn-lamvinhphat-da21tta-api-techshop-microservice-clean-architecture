using System.Text.RegularExpressions;
using BuildingBlocks.Abstractions.Entities;
using BuildingBlocks.Error;
using BuildingBlocks.Results;

namespace ProductService.Domain.ValueObjects
{
    public sealed class SKU : ValueObject
    {
        public string Value { get; }

        private SKU(string value)
        {
            Value = value;
        }
        
        public static Result<SKU> Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return Result.Failure<SKU>(Error.Validation("SKU.Empty", "SKU cannot be empty."));

            if (!IsValidSKU(value))
                return Result.Failure<SKU>(Error.Validation("SKU.InvalidFormat", "SKU should be alphanumeric and 5-20 characters long."));

            return Result.Success(new SKU(value.ToUpper()));
        }

        private static bool IsValidSKU(string value)
        {
            var regex = new Regex(@"^[A-Z0-9]{5,20}$", RegexOptions.IgnoreCase);
            return regex.IsMatch(value);
        }
        
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public override string ToString() => Value;
    }
}