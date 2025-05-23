using BuildingBlocks.Abstractions.Entities;

namespace OrderService.Domain.Entities
{
    public class OrderItem : Entity
    {
        private OrderItem(int orderId, int productId, int quantity, decimal price)
        {
            OrderId = orderId;
            ProductId = productId;
            Quantity = quantity;
            Price = price;
        }

        public int OrderId { get; private set; }
        public int ProductId { get; private set; }
        public int Quantity { get; }
        public decimal Price { get; }

        public decimal TotalPrice => Price * Quantity;

        public static OrderItem Create(int orderId, int productId, int quantity, decimal price)
        {
            return new OrderItem(orderId, productId, quantity, price);
        }
    }
}