using BuildingBlocks.CQRS;
using OrderService.Domain.Enum;

namespace OrderService.Application.Commands.Orders.PayOS
{
    public record CreateLinkCommand(
        int CartId,
        string ReceiverName,
        string ReceiverPhone,
        string ReceiverAddress,
        string? Note,
        string? SessionId,
        PaymentMethod PaymentMethod,
        long Amount,
        string ReturnUrl,
        string CancelUrl) : ICommand<string>;
}