using BuildingBlocks.Abstractions.Entities;
using BuildingBlocks.Results;
using ProductService.Domain.Errors;

namespace ProductService.Domain.Entities
{
    public class ProductImage : Entity
    {
        public ProductImage(int productId, string imageUrl, bool? isMain, int? sortOrder)
        {
            ProductId = productId;
            ImageUrl = imageUrl;
            IsMain = isMain ?? false;
            SortOrder = sortOrder ?? 0;
        }

        private ProductImage()
        {
        }

        public int ProductId { get; private set; }
        public string ImageUrl { get; private set; }
        public bool? IsMain { get; private set; }
        public int? SortOrder { get; private set; }

        public static Result<ProductImage> Create(int productId, string imageUrl, bool isMain, int sortOrder)
        {
            if (string.IsNullOrWhiteSpace(imageUrl))
                return Result.Failure<ProductImage>(ProductImageError.ProductImageInvalid);

            return new ProductImage(productId, imageUrl, isMain, sortOrder);
        }
    }
}