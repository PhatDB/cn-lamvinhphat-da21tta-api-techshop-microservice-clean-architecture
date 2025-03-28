using BuildingBlocks.Abstractions.Entities;

namespace OrderService.Domain.Entities
{
    public class OrderItem : Entity
    {
        public OrderItem(int productId, int quantity, string productName, decimal unitPrice)
        {
            ProductId = productId;
            Quantity = quantity;
            ProductName = productName;
            UnitPrice = unitPrice;
            TotalPrice = quantity * unitPrice;
        }

        public int OrderId { get; private set; }
        public int ProductId { get; private set; }
        public int Quantity { get; private set; }
        public string ProductName { get; private set; }
        public decimal UnitPrice { get; private set; }
        public decimal TotalPrice { get; private set; }
    }
}