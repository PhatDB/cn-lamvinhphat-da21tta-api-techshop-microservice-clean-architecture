using BuildingBlocks.Error;

namespace ProductService.Domain.Errors
{
    public static class BrandError
    {
        public static readonly Error BrandAlreadyExists =
            Error.Conflict("Brand.AlreadyExists", "Brand already exists.");

        public static readonly Error BrandNotFound = Error.NotFound("Brand.NotFound", "Brand not found.");
    }
}