using BuildingBlocks.Abstractions.Aggregates;
using BuildingBlocks.Abstractions.Entities;
using BuildingBlocks.Results;
using CartService.Domain.Errors;

namespace CartService.Domain.Entities
{
    public class Cart : Entity, IAggregateRoot
    {
        private readonly List<CartItem> _cartItems;

        public Cart(int customerId)
        {
            CustomerId = customerId;
            CreatedAt = DateTime.UtcNow;
            _cartItems = new List<CartItem>();
        }

        public int CustomerId { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public IReadOnlyCollection<CartItem> CartItems => _cartItems.AsReadOnly();

        public static Result<Cart> Create(int customerId)
        {
            if (customerId <= 0)
                return Result.Failure<Cart>(CartError.UserIdIsInvalid);

            return Result.Success(new Cart(customerId));
        }

        public Result AddItem(int productId, int quantity, decimal price)
        {
            if (quantity <= 0)
                return Result.Failure(CartError.InvalidQuantity);

            CartItem? existingItem = _cartItems.FirstOrDefault(i => i.ProductId == productId);
            if (existingItem is not null)
            {
                existingItem.IncreaseQuantity(quantity);
                return Result.Success();
            }

            Result<CartItem> createResult = CartItem.Create(Id, productId, quantity, price);
            if (createResult.IsFailure)
                return Result.Failure(createResult.Error);

            _cartItems.Add(createResult.Value);
            return Result.Success();
        }

        public Result AddOrUpdateItem(int productId, int quantity, decimal price)
        {
            if (quantity <= 0)
                return Result.Failure(CartError.InvalidQuantity);

            CartItem? existingItem = _cartItems.FirstOrDefault(i => i.ProductId == productId);
            if (existingItem is not null)
            {
                existingItem.UpdateQuantity(quantity);
                return Result.Success();
            }

            Result<CartItem> createResult = CartItem.Create(Id, productId, quantity, price);
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