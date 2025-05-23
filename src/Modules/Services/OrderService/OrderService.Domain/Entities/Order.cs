using BuildingBlocks.Abstractions.Aggregates;
using BuildingBlocks.Abstractions.Entities;
using BuildingBlocks.Error;
using BuildingBlocks.Results;
using OrderService.Domain.Enum;

namespace OrderService.Domain.Entities
{
    public class Order : Entity, IAggregateRoot
    {
        private readonly List<OrderItem> _orderItems;

        private Order(int customerId, OrderStatus status, decimal totalAmount)
        {
            CustomerId = customerId;
            Status = OrderStatus.Submitted;
            CreatedAt = DateTime.UtcNow;
            TotalAmount = totalAmount;
            _orderItems = new List<OrderItem>();
        }

        public int CustomerId { get; private set; }
        public OrderStatus Status { get; private set; }
        public decimal TotalAmount { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();

        public static Result<Order> Create(int customerId, OrderStatus status, decimal totalAmount)
        {
            if (customerId <= 0)
                return Result.Failure<Order>(Error.Validation("Order.InvalidCustomerId",
                    "CustomerId must be greater than 0."));

            if (totalAmount < 0)
                return Result.Failure<Order>(Error.Validation("Order.InvalidTotal",
                    "Total amount cannot be negative."));

            Order order = new(customerId, status, totalAmount);

            return Result.Success(order);
        }


        public void AddItem(int productId, int quantity, decimal price)
        {
            OrderItem item = OrderItem.Create(Id, productId, quantity, price);
            _orderItems.Add(item);
        }

        public void UpdateTotalAmount()
        {
            TotalAmount = _orderItems.Sum(i => i.TotalPrice);
        }

        public void SetAwaitingValidationStatus()
        {
            if (Status == OrderStatus.Submitted)
                Status = OrderStatus.AwaitingValidation;
        }

        public void SetStockConfirmedStatus()
        {
            if (Status == OrderStatus.AwaitingValidation)
                Status = OrderStatus.StockConfirmed;
        }

        public Result SetPaidStatus()
        {
            if (Status != OrderStatus.Submitted)
                return StatusChangeException(OrderStatus.Paid);

            Status = OrderStatus.Paid;
            return Result.Success();
        }

        public Result SetShippedStatus()
        {
            if (Status != OrderStatus.Paid)
                return StatusChangeException(OrderStatus.Shipped);

            Status = OrderStatus.Shipped;
            return Result.Success();
        }

        public Result SetCancelledStatus()
        {
            if (Status == OrderStatus.Paid || Status == OrderStatus.Shipped)
                return StatusChangeException(OrderStatus.Cancelled);

            Status = OrderStatus.Cancelled;
            return Result.Success();
        }

        public void SetCancelledStatusWhenStockIsRejected(IEnumerable<int> rejectedProductIds)
        {
            if (Status == OrderStatus.AwaitingValidation)
            {
                Status = OrderStatus.Cancelled;

                IEnumerable<int> rejectedItems = OrderItems.Where(i => rejectedProductIds.Contains(i.ProductId))
                    .Select(i => i.ProductId);

                string description = string.Join(", ", rejectedItems);
            }
        }

        private Result StatusChangeException(OrderStatus toStatus)
        {
            return Result.Failure(Error.Problem("Order.Status",
                $"Cannot change order status from {Status} to {toStatus}."));
        }
    }
}