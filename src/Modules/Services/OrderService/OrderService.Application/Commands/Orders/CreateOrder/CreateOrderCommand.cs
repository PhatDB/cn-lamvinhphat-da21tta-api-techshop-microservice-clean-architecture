using BuildingBlocks.CQRS;
using OrderService.Domain.Enum;

namespace OrderService.Application.Commands.Orders.CreateOrder
{
    public record CreateOrderCommand(
        int CartId,
        string ReceiverName,
        string ReceiverPhone,
        string ReceiverAddress,
        string? Note,
        string? SessionId,
        PaymentMethod PaymentMethod) : ICommand<int>;
}