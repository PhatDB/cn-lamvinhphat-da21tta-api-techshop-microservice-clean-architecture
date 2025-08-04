using System.Text.Json;
using BuildingBlocks.CQRS;

namespace OrderService.Application.Commands.Orders.PayOS
{
    public record WebHookCommand(JsonElement DataEl, string Signature) : ICommand;
}