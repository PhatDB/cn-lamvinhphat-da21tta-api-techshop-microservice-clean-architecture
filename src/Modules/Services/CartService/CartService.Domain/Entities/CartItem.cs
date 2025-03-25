using BuildingBlocks.Abstractions.Entities;
using BuildingBlocks.Results;
using CartService.Domain.Errors;

namespace CartService.Domain.Entities
{
    public class CartItem : Entity
    {
        private CartItem(int cartId, int productId, int quantity, decimal unitPrice)
        {
            CartId = cartId;
            ProductId = productId;
            Quantity = quantity;
            UnitPrice = unitPrice;
        }

        private CartItem()
        {
        }

        public int CartId { get; private set; }
        public int ProductId { get; private set; }
        public int Quantity { get; private set; }
        public decimal UnitPrice { get; }
        public decimal TotalPrice => Quantity * UnitPrice;

        public void IncreaseQuantity(int amount)
        {
            Quantity += amount;
        }

        public void UpdateQuantity(int newQuantity)
        {
            Quantity = newQuantity;
        }

        public static Result<CartItem> Create(int cartId, int productId, int quantity, decimal unitPrice)
        {
            if (quantity <= 0)
                return Result.Failure<CartItem>(CartError.InvalidQuantity);

            if (unitPrice <= 0)
                return Result.Failure<CartItem>(CartError.InvalidUnitPrice);

            return Result.Success(new CartItem(cartId, productId, quantity, unitPrice));
        }
    }
}