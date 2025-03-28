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

        public Order(int? userId, string street, string city, string district, string ward, string? zipCode, string phoneNumber, string buyerName)
        {
            UserId = userId;
            Street = street;
            City = city;
            District = district;
            Ward = ward;
            ZipCode = zipCode;
            PhoneNumber = phoneNumber;
            OrderStatus = OrderStatus.Submitted;
            CreatedAt = DateTime.UtcNow;
            TotalAmount = 0;
            BuyerName = buyerName;
            _orderItems = new List<OrderItem>();
        }

        public int? UserId { get; private set; }
        public string Street { get; private set; }
        public string City { get; private set; }
        public string District { get; private set; }
        public string Ward { get; private set; }
        public string? ZipCode { get; private set; }
        public string PhoneNumber { get; private set; }
        public decimal TotalAmount { get; private set; }
        public string BuyerName { get; private set; }
        public OrderStatus OrderStatus { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();

        public void AddItem(int productId, int quantity, string productName, decimal unitPrice)
        {
            OrderItem orderItem = new(productId, quantity, productName, unitPrice);
            _orderItems.Add(orderItem);
            UpdateTotalAmount();
        }

        public void UpdateTotalAmount()
        {
            TotalAmount = 0;
            foreach (OrderItem item in _orderItems)
                TotalAmount += item.TotalPrice;
        }

        public void SetAwaitingValidationStatus()
        {
            if (OrderStatus == OrderStatus.Submitted) OrderStatus = OrderStatus.AwaitingValidation;
        }

        public void SetStockConfirmedStatus()
        {
            if (OrderStatus == OrderStatus.AwaitingValidation) OrderStatus = OrderStatus.StockConfirmed;
        }

        public Result SetPaidStatus()
        {
            if (OrderStatus != OrderStatus.Submitted)
                return StatusChangeException(OrderStatus.Paid);

            OrderStatus = OrderStatus.Paid;
            return Result.Success();
        }

        public Result SetShippedStatus()
        {
            if (OrderStatus != OrderStatus.Paid) StatusChangeException(OrderStatus.Shipped);

            OrderStatus = OrderStatus.Shipped;
            return Result.Success();
        }

        public Result SetCancelledStatus()
        {
            if (OrderStatus == OrderStatus.Paid || OrderStatus == OrderStatus.Shipped)
                StatusChangeException(OrderStatus.Cancelled);

            OrderStatus = OrderStatus.Cancelled;
            return Result.Success();
        }

        public void SetCancelledStatusWhenStockIsRejected(IEnumerable<int> orderStockRejectedItems)
        {
            if (OrderStatus == OrderStatus.AwaitingValidation)
            {
                OrderStatus = OrderStatus.Cancelled;

                IEnumerable<int> itemsStockRejectedProductNames = OrderItems.Where(c => orderStockRejectedItems.Contains(c.ProductId)).Select(c => c.ProductId);

                string itemsStockRejectedDescription = string.Join(", ", itemsStockRejectedProductNames);
            }
        }

        private Result StatusChangeException(OrderStatus orderStatusToChange)
        {
            return Result.Failure(Error.Problem("Order.Status", "$\"Is not possible to change the order status from {OrderStatus} to {orderStatusToChange}.\""));
        }
    }
}