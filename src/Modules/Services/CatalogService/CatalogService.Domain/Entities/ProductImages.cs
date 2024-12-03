using BuildingBlocks.Extensions;
using CatalogService.Domain.Abstractions.Entities;
using CatalogService.Domain.ValueObjects;

namespace CatalogService.Domain.Entities
{
    public class ProductImages : Entity<ProductImageId>
    {
        private ProductImages()
        {
        }

        public ProductId ProductId { get; private set; }
        public string ImageUrl { get; private set; }
        public string? AltText { get; private set; }
        public DateTime CreatedDate { get; private set; }
        public DateTime? UpdatedDate { get; private set; }

        public static ProductImages Create(ProductImageId id, ProductId productId, string imageUrl, string? altText, DateTime createdDate)
        {
            Ensure.NotNullOrEmpty(imageUrl);
            return new ProductImages
            {
                Id = id,
                ProductId = productId,
                ImageUrl = imageUrl,
                AltText = altText,
                CreatedDate = createdDate
            };
        }

        public void Update(string imageUrl, string? altText, DateTime updatedDate)
        {
            ImageUrl = imageUrl;
            AltText = altText;
            UpdatedDate = updatedDate;
        }
    }
}