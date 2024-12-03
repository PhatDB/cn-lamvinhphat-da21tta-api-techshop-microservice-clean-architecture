using BuildingBlocks.Extensions;

namespace CatalogService.Domain.ValueObjects
{
    public record CategoryId
    {
        private CategoryId(Guid value)
        {
            Value = value;
        }

        public Guid Value { get; }

        public static CategoryId Create(Guid value)
        {
            Ensure.NotNullOrEmpty(value.ToString());
            return new CategoryId(value);
        }
    }
}