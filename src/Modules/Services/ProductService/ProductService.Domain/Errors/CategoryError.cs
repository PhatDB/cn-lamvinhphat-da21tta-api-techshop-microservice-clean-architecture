using BuildingBlocks.Error;

namespace ProductService.Domain.Errors
{
    public static class CategoryError
    {
        public static readonly Error CategoryNameInvalid =
            Error.Validation("Category.NameInvalid", "Category name cannot be empty.");

        public static readonly Error CategoryNotFound =
            Error.NotFound("Category.NotFound", "Category not found.");

        public static readonly Error CategoryAlreadyDeleted =
            Error.Conflict("Category.AlreadyDeleted", "Category is already deleted.");
    }
}