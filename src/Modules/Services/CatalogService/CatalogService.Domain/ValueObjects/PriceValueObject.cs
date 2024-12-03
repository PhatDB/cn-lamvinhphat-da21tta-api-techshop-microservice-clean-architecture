using BuildingBlocks.Extensions;

namespace CatalogService.Domain.ValueObjects
{
    public record PriceValueObject
    {
        private PriceValueObject(decimal price, decimal? discountPrice)
        {
            if (discountPrice.HasValue && discountPrice.Value > price) throw new ArgumentException("Discount price cannot be greater than the original price.");

            Price = price;
            DiscountPrice = discountPrice;
        }

        protected PriceValueObject()
        {
        }

        public decimal Price { get; }
        public decimal? DiscountPrice { get; }

        public static PriceValueObject Create(decimal price, decimal? discountPrice = null)
        {
            Ensure.GreaterThanZero(price);
            if (discountPrice.HasValue) Ensure.GreaterThanZero(discountPrice.Value);

            return new PriceValueObject(price, discountPrice);
        }
    }
}