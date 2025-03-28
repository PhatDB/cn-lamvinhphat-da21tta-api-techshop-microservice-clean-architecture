using BuildingBlocks.CQRS;

namespace OrderService.Application.Commands.Orders.CancelOrder
{
    public record CancelOrderCommand(int OrderId) : ICommand;
}