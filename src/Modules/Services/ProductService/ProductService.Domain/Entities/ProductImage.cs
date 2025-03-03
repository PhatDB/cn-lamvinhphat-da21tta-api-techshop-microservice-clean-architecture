using BuildingBlocks.Abstractions.Entities;
using BuildingBlocks.Results;
using ProductService.Domain.Errors;

namespace ProductService.Domain.Entities
{
    public class ProductImage : Entity
    {
        public ProductImage(int productId, string imageUrl, int position, string? title)
        {
            ProductId = productId;
            ImageUrl = imageUrl;
            Position = position;
            Title = title;
        }

        public int ProductId { get; private set; }
        public string ImageUrl { get; private set; }
        public string? Title { get; private set; }
        public int Position { get; private set; }

        public static Result<ProductImage> Create(
            int productId, string imageUrl, int position, string? title)
        {
            if (string.IsNullOrWhiteSpace(imageUrl))
                return Result.Failure<ProductImage>(ProductImageError
                    .ProductImageInvalid);

            return new ProductImage(productId, imageUrl, position, title);
        }

        public void UpdateImage(string newImageUrl, int newPosition)
        {
            ImageUrl = newImageUrl;
            Position = newPosition;
        }
    }
}