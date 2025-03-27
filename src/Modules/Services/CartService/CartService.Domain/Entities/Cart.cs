using BuildingBlocks.Abstractions.Aggregates;
using BuildingBlocks.Abstractions.Entities;
using BuildingBlocks.Results;
using CartService.Domain.Errors;

namespace CartService.Domain.Entities
{
    public class Cart : Entity, IAggregateRoot
    {
        private readonly List<CartItem> _cartItems;

        private Cart()
        {
            _cartItems = new List<CartItem>();
        }

        public Cart(int userId) : this()
        {
            UserId = userId;
            CreatedAt = DateTime.UtcNow;
        }

        public int UserId { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public IReadOnlyCollection<CartItem> CartItems => _cartItems.AsReadOnly();

        public static Result<Cart> Create(int userId)
        {
            if (userId <= 0)
                return Result.Failure<Cart>(CartError.UserIdIsInvalid);

            return Result.Success(new Cart(userId));
        }

        public Result AddItem(int productId, string productName, string imgUrl, int quantity, decimal unitPrice)
        {
            if (quantity <= 0)
                return Result.Failure(CartError.InvalidQuantity);

            CartItem? existingItem = _cartItems.FirstOrDefault(i => i.ProductId == productId);
            if (existingItem is not null)
            {
                existingItem.IncreaseQuantity(quantity);
                return Result.Success();
            }

            Result<CartItem> createResult = CartItem.Create(Id, productId, productName, imgUrl, quantity, unitPrice);
            if (createResult.IsFailure)
                return Result.Failure(createResult.Error);

            _cartItems.Add(createResult.Value);
            return Result.Success();
        }

        public Result AddOrUpdateItem(int productId, string productName, string imgUrl, int quantity, decimal unitPrice)
        {
            if (quantity <= 0)
                return Result.Failure(CartError.InvalidQuantity);

            CartItem? existingItem = _cartItems.FirstOrDefault(i => i.ProductId == productId);
            if (existingItem is not null)
            {
                existingItem.UpdateQuantity(quantity);
                return Result.Success();
            }

            Result<CartItem> createResult = CartItem.Create(Id, productId, productName, imgUrl, quantity, unitPrice);
            if (createResult.IsFailure)
                return Result.Failure(createResult.Error);

            _cartItems.Add(createResult.Value);
            return Result.Success();
        }

        public Result RemoveItem(int productId)
        {
            CartItem? item = _cartItems.FirstOrDefault(i => i.ProductId == productId);
            if (item is null)
                return Result.Failure(CartError.ItemNotFound);

            _cartItems.Remove(item);
            return Result.Success();
        }

        public Result ClearCart()
        {
            _cartItems.Clear();
            return Result.Success();
        }
    }
}