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

        private Order(
            int? customerId, OrderStatus status, decimal totalAmount, string receiverName, string receiverPhone,
            string receiverAddress, string? note, string? sessionId, PaymentMethod paymentMethod)
        {
            CustomerId = customerId;
            Status = status;
            TotalAmount = totalAmount;
            CreatedAt = DateTime.UtcNow;
            ReceiverName = receiverName;
            ReceiverPhone = receiverPhone;
            ReceiverAddress = receiverAddress;
            Note = note;
            SessionId = sessionId;
            PaymentMethod = paymentMethod;
            _orderItems = new List<OrderItem>();
        }

        public int? CustomerId { get; private set; }
        public OrderStatus Status { get; private set; }
        public decimal TotalAmount { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public string ReceiverName { get; private set; }
        public string ReceiverPhone { get; private set; }
        public string ReceiverAddress { get; private set; }
        public string? Note { get; private set; }
        public string? SessionId { get; private set; }
        public PaymentMethod PaymentMethod { get; }

        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();

        public static Result<Order> Create(
            int? customerId, OrderStatus status, decimal totalAmount, string receiverName, string receiverPhone,
            string receiverAddress, string? note, string? sessionId, PaymentMethod paymentMethod)
        {
            if (totalAmount < 0)
                return Result.Failure<Order>(Error.Validation("Order.InvalidTotal",
                    "Total amount cannot be negative."));

            if (string.IsNullOrWhiteSpace(receiverName))
                return Result.Failure<Order>(
                    Error.Validation("Order.MissingReceiverName", "Receiver name is required."));

            Order order = new(customerId, status, totalAmount, receiverName, receiverPhone, receiverAddress, note,
                sessionId, paymentMethod);

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

        public Result SetConfirmedStatus()
        {
            if (Status != OrderStatus.AwaitingValidation)
                return StatusChangeException(OrderStatus.Confirmed);

            Status = OrderStatus.Confirmed;
            return Result.Success();
        }

        public Result SetPaidStatus()
        {
            if (PaymentMethod != PaymentMethod.BankTransfer)
                return Result.Failure(Error.Validation("Order.InvalidPaymentMethod",
                    "Chỉ đơn hàng thanh toán chuyển khoản mới được đánh dấu là đã thanh toán."));

            if (Status != OrderStatus.Confirmed)
                return StatusChangeException(OrderStatus.Paid);

            Status = OrderStatus.Paid;
            return Result.Success();
        }

        public Result SetShippingStatus()
        {
            if (PaymentMethod == PaymentMethod.BankTransfer && Status != OrderStatus.Paid)
                return StatusChangeException(OrderStatus.Shipping);

            if (PaymentMethod == PaymentMethod.COD && Status != OrderStatus.Confirmed)
                return StatusChangeException(OrderStatus.Shipping);

            Status = OrderStatus.Shipping;
            return Result.Success();
        }

        public Result SetDeliveredStatus()
        {
            if (Status != OrderStatus.Shipping)
                return StatusChangeException(OrderStatus.Delivered);

            Status = OrderStatus.Delivered;
            return Result.Success();
        }

        public Result SetCancelledStatus()
        {
            if (Status == OrderStatus.Shipping || Status == OrderStatus.Delivered)
                return StatusChangeException(OrderStatus.Cancelled);

            Status = OrderStatus.Cancelled;
            return Result.Success();
        }

        private Result StatusChangeException(OrderStatus toStatus)
        {
            return Result.Failure(Error.Problem("Order.Status",
                $"Cannot change order status from {Status} to {toStatus}."));
        }
    }
}