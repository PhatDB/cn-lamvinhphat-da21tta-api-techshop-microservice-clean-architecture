using BuildingBlocks.CQRS;
using OrderService.Application.DTOs;

namespace OrderService.Application.Queries.Orders
{
    public record GetOrderByUserIdQuery(int UserId) : IQuery<OrderDTO>;
}