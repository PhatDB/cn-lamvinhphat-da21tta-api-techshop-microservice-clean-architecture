using BuildingBlocks.CQRS;
using CartService.Application.DTOs;

namespace CartService.Application.Queries.Cart
{
    public record GetCartByUserIdQuery(int UserId) : IQuery<CartDTO>;
}