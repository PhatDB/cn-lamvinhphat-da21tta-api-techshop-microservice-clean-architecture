using BuildingBlocks.Error;

namespace ProductService.Domain.Errors
{
    public static class ProductImageError
    {
        public static readonly Error ProductImageInvalid =
            Error.Validation("ProductImage.Invalid", "Image URL cannot be empty.");

        public static readonly Error ProductImageNotFound =
            Error.NotFound("ProductImage.NotFound",
                "No matching images found for deletion.");

        public static readonly Error ProductImageBase64Invalid =
            Error.Validation("Base64.Validation", "Invalid image content");
    }
}