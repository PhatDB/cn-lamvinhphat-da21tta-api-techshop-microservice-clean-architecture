using BuildingBlocks.Error;

namespace CartService.Domain.Errors
{
    public static class CartError
    {
        public static readonly Error UserIdIsInvalid = Error.Validation("Cart.UserIdIsInvalid", "User id is invalid.");
        public static readonly Error ItemNotFound = Error.NotFound("Cart.ItemNotFound", "Cart item is not found.");
        public static readonly Error InvalidProductId = Error.Validation("Cart.InvalidProductId", "Product ID must be greater than 0.");
        public static readonly Error InvalidQuantity = Error.Validation("Cart.InvalidQuantity", "Quantity must be greater than 0.");
        public static readonly Error InvalidUnitPrice = Error.Validation("Cart.InvalidUnitPrice", "Unit price must be greater than 0.");
    }
}