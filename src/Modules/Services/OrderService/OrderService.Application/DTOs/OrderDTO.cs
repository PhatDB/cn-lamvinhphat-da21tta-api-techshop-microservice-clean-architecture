using OrderService.Domain.Enum;

namespace OrderService.Application.DTOs
{
    public class OrderDTO
    {
        public int? UserId { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Ward { get; set; }
        public string? ZipCode { get; set; }
        public string PhoneNumber { get; set; }
        public decimal TotalAmount { get; set; }
        public string BuyerName { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<OrderItemDTO> OrderItems { get; set; }
    }
}