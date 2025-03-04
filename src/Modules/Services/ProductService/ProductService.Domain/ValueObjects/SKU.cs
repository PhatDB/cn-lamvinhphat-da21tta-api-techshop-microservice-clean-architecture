using System.Text.RegularExpressions;
using BuildingBlocks.Abstractions.Entities;
using BuildingBlocks.Results;
using ProductService.Domain.Errors;

namespace ProductService.Domain.ValueObjects
{
    public sealed class SKU : ValueObject
    {
        private SKU(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public static Result<SKU> Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return Result.Failure<SKU>(ProductError.ProductSkuEmpty);

            if (!IsValidSKU(value))
                return Result.Failure<SKU>(ProductError.ProductSkuInvalidFormat);

            return Result.Success(new SKU(value.ToUpper()));
        }

        private static bool IsValidSKU(string value)
        {
            Regex regex = new(@"^[A-Z0-9]{5,20}$", RegexOptions.IgnoreCase);
            return regex.IsMatch(value);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}