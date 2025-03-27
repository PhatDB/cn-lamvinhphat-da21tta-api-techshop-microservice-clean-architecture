using BuildingBlocks.Abstractions.Aggregates;
using BuildingBlocks.Abstractions.Entities;

namespace OrderService.Domain.Entities
{
    public class Order : Entity, IAggregateRoot
    {
        private readonly List<OrderItem> _orderItems;

        public Order(int userId, int userAddressId)
        {
            UserId = userId;
            UserAddressId = userAddressId;
            PaymentStatus = 0;
            CreatedAt = DateTime.UtcNow;
            TotalAmount = 0;
            _orderItems = new List<OrderItem>();
        }

        public int UserId { get; private set; }
        public int UserAddressId { get; private set; }
        public decimal TotalAmount { get; private set; }
        public int PaymentStatus { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();

        public void AddItem(int productId, int quantity, decimal unitPrice)
        {
            OrderItem orderItem = new(productId, quantity, unitPrice);
            _orderItems.Add(orderItem);
            UpdateTotalAmount();
        }

        public void UpdateTotalAmount()
        {
            TotalAmount = 0;
            foreach (OrderItem item in _orderItems) TotalAmount += item.TotalPrice;
        }

        public void UpdatePaymentStatus(int newStatus)
        {
            PaymentStatus = newStatus;
        }
    }
}