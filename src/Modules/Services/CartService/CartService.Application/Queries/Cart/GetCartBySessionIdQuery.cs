using BuildingBlocks.CQRS;
using CartService.Application.DTOs;

namespace CartService.Application.Queries.Cart
{
    public record GetCartBySessionIdQuery(string SessionId) : IQuery<CartDTO>;
}