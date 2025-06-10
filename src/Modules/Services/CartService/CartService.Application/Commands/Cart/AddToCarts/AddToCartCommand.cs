using BuildingBlocks.CQRS;

namespace CartService.Application.Commands.Cart.AddToCarts
{
    public record AddToCartCommand(int? CustomerId, string? SessionId, int ProductId, int Quantity) : ICommand<int>;
}