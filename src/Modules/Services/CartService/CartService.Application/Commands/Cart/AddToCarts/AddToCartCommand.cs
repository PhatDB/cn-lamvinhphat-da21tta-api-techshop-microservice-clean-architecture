using BuildingBlocks.CQRS;

namespace CartService.Application.Commands.Cart.AddToCarts
{
    public record AddToCartCommand(int UserId, int ProductId, int Quantity) : ICommand<int>;
}