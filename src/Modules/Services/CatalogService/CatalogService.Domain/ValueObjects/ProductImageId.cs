using BuildingBlocks.Extensions;

namespace CatalogService.Domain.ValueObjects
{
    public class ProductImageId
    {
        private ProductImageId(Guid value)
        {
            Value = value;
        }

        public Guid Value { get; }

        public static ProductImageId Create(Guid value)
        {
            Ensure.NotNullOrEmpty(value.ToString());
            return new ProductImageId(value);
        }
    }
}