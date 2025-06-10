using BuildingBlocks.CQRS;

namespace OrderService.Application.Commands.Orders.SetShippingOrder
{
    public record SetShippingOrderCommand(int OrderId) : ICommand;
}