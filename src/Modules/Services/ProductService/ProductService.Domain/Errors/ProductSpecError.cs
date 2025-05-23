using BuildingBlocks.Error;

namespace ProductService.Domain.Errors
{
    public static class ProductSpecError
    {
        public static readonly Error SpecNameInvalid =
            Error.Validation("ProductSpec.Invalid", "Spec Name cannot be empty.");

        public static readonly Error ProductSpecsNull =
            Error.Validation("ProductSpec.Null", "Product Specs cannot be null.");

        public static readonly Error ProductSpecsNotFound =
            Error.Validation("ProductSpec.NotFound", "Product Specs cannot be found.");
    }
}