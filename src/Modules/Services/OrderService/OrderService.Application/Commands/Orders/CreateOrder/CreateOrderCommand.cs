using BuildingBlocks.CQRS;
using OrderService.Application.DTOs;

namespace OrderService.Application.Commands.Orders.CreateOrder
{
    public class CreateOrderCommand : ICommand<int>
    {
        public int UserId { get; set; }
        public List<OrderItemDTO> OrderItems { get; set; }
        public string PaymentStatus { get; set; }
    }
}