using BuildingBlocks.CQRS;

namespace OrderService.Application.Commands.Orders.SetConfirmedOrder
{
    public record SetConfirmedOrderCommand(int OrderId) : ICommand;
}