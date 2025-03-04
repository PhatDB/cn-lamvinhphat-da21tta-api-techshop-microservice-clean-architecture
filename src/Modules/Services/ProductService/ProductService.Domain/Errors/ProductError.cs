using BuildingBlocks.Error;

namespace ProductService.Domain.Errors
{
    public static class ProductError
    {
        public static readonly Error ProductNameInvalid =
            Error.Validation("Product.NameInvalid", "Product name cannot be empty.");

        public static readonly Error ProductPriceInvalid =
            Error.Validation("Product.PriceInvalid", "Price must be greater than zero.");

        public static readonly Error ProductInsufficientStock =
            Error.Validation("Product.InsufficientStock", "Not enough stock available.");

        public static readonly Error ProductNotFound =
            Error.NotFound("Product.NotFound", "Product not found.");

        public static readonly Error ProductAlreadyDeleted =
            Error.Conflict("Product.AlreadyDeleted", "Product is already deleted.");

        public static readonly Error ProductSkuEmpty =
            Error.Validation("SKU.Empty", "SKU cannot be empty.");

        public static readonly Error ProductSkuInvalidFormat =
            Error.Validation("SKU.InvalidFormat",
                "SKU should be alphanumeric and 5-20 characters long.");

        public static readonly Error ProductSkuDuplicate =
            Error.Validation("Product.SkuDuplicate",
                "SKU already exists for another product.");
    }
}