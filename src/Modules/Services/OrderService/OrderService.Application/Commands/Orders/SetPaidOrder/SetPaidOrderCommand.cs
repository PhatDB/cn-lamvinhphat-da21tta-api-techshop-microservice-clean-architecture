using BuildingBlocks.CQRS;

namespace OrderService.Application.Commands.Orders.SetPaidOrder
{
    public record SetPaidOrderCommand(int OrderId) : ICommand;
}