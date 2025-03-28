using BuildingBlocks.CQRS;

namespace OrderService.Application.Commands.Orders.CreateOrder
{
    public record CreateOrderCommand(int UserId, string Street, string City, string District, string Ward, string? ZipCode, string PhoneNumber, string BuyerName) : ICommand<int>;
}