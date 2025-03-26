using BuildingBlocks.CQRS;

namespace CartService.Application.Commands.Cart.RemoveItems
{
    public record RemoveItemCommand(int CartId, int ProductId) : ICommand;
}