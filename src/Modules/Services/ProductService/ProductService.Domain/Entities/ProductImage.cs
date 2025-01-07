using BuildingBlocks.Abstractions.Entities;
using BuildingBlocks.Results;
using ProductService.Domain.Errors;

namespace ProductService.Domain.Entities
{
    public class ProductImage : Entity
    {
        private ProductImage(int productId, string imageUrl, string? altText)
        {
            ProductId = productId;
            ImageUrl = imageUrl;
            AltText = altText;
            CreatedDate = DateTime.UtcNow;
            UpdatedDate = DateTime.UtcNow;
        }

        private ProductImage()
        {
        }

        public int ProductId { get; private set; }
        public string ImageUrl { get; private set; }
        public string? AltText { get; private set; }
        public DateTime CreatedDate { get; private set; }
        public DateTime UpdatedDate { get; private set; }

        public static Result<ProductImage> Create(int productId, string imageUrl, string? altText = null)
        {
            if (string.IsNullOrWhiteSpace(imageUrl))
                return Result.Failure<ProductImage>(ProductImageError.ProductImageInvalid);

            return new ProductImage(productId, imageUrl, altText);
        }

        public void UpdateImage(string newImageUrl, string? newAltText = null)
        {
            ImageUrl = newImageUrl;
            AltText = newAltText;
            UpdatedDate = DateTime.UtcNow;
        }
    }
}