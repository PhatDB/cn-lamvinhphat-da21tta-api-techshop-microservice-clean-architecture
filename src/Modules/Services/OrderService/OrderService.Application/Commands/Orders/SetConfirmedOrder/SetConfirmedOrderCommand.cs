using BuildingBlocks.CQRS;

namespace OrderService.Application.Commands.Orders.SetComfirmedOrder
{
    public record SetConfirmedOrderCommand(int OrderId) : ICommand;
}