using BuildingBlocks.Abstractions.Entities;
using BuildingBlocks.Results;
using CartService.Domain.Errors;

namespace CartService.Domain.Entities
{
    public class CartItem : Entity
    {
        private CartItem(int cartId, int productId, int quantity, decimal price)
        {
            CartId = cartId;
            ProductId = productId;
            Quantity = quantity;
            Price = price;
        }

        public int CartId { get; private set; }
        public int ProductId { get; private set; }
        public int Quantity { get; private set; }
        public decimal Price { get; private set; }

        public void IncreaseQuantity(int amount)
        {
            Quantity += amount;
        }

        public void UpdateQuantity(int newQuantity)
        {
            Quantity = newQuantity;
        }

        public static Result<CartItem> Create(int cartId, int productId, int quantity, decimal price)
        {
            if (quantity <= 0)
                return Result.Failure<CartItem>(CartError.InvalidQuantity);

            if (price <= 0)
                return Result.Failure<CartItem>(CartError.InvalidUnitPrice);

            return Result.Success(new CartItem(cartId, productId, quantity, price));
        }
    }
}