using BuildingBlocks.Error;

namespace ProductService.Domain.Errors
{
    public static class ColorError
    {
        public static readonly Error ColorInvalidName =
            Error.Validation("Color.EmptyName", "Color name cannot be empty.");
    }
}