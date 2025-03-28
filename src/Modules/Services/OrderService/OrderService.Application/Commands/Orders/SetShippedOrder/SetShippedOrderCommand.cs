using BuildingBlocks.CQRS;

namespace OrderService.Application.Commands.Orders.SetShippedOrder
{
    public record SetShippedOrderCommand(int OrderId) : ICommand;
}