using BuildingBlocks.CQRS;

namespace OrderService.Application.Commands.Orders.SetDeliveredOrder
{
    public record SetDeliveredOrderCommand(int OrderId) : ICommand;
}