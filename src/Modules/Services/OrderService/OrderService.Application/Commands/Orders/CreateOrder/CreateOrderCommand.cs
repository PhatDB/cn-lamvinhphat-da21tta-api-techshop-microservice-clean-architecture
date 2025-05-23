using BuildingBlocks.CQRS;

namespace OrderService.Application.Commands.Orders.CreateOrder
{
    public record CreateOrderCommand(int CartId) : ICommand<int>;
}