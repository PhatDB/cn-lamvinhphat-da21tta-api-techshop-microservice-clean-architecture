using BuildingBlocks.Error;

namespace ProductService.Domain.Errors
{
    public static class ProductColorError
    {
        public static readonly Error ProductColorDuplicate =
            Error.Conflict("ProductColor.Duplicate",
                "Color already exists for this product.");

        public static readonly Error ProductColorNotFound =
            Error.NotFound("ProductColor.NotFound", "Color not found for this product.");

        public static readonly Error ProductColorNullColor =
            Error.Validation("ProductColor.NullColor", "Color cannot be null.");
    }
}