using BuildingBlocks.Extensions;

namespace CatalogService.Domain.ValueObjects
{
    public record Name
    {
        public Name(string? value)
        {
            Ensure.NotNullOrEmpty(value);

            Value = value;
        }

        public string Value { get; }
    }
}