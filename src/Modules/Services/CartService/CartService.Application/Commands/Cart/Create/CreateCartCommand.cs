using BuildingBlocks.CQRS;

namespace CartService.Application.Commands.Cart.Create
{
    public record CreateCartCommand(int UserId, int ProductId, int Quantity) : ICommand<int>;
}