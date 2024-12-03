using BuildingBlocks.Extensions;

namespace CatalogService.Domain.ValueObjects
{
    public record ProductId
    {
        private ProductId(Guid value)
        {
            Value = value;
        }

        public Guid Value { get; }

        public static ProductId Create(Guid value)
        {
            Ensure.NotNullOrEmpty(value.ToString());
            return new ProductId(value);
        }
    }
}