using BuildingBlocks.CQRS;

namespace CartService.Application.Commands.Cart.ClearCart
{
    public record ClearCartCommand(int CartId) : ICommand;
}