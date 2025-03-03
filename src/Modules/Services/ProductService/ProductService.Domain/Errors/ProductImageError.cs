using BuildingBlocks.Error;

namespace ProductService.Domain.Errors
{
    public static class ProductImageError
    {
        public static readonly Error ProductImageInvalid =
            Error.Validation("ProductImage.Validation", "ProductImage is Invalid");
    }
}