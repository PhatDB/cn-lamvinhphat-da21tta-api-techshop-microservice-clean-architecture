using BuildingBlocks.CQRS;
using OrderService.Application.DTOs;

namespace OrderService.Application.Queries.Orders
{
    public record GetOrderDetailQuery(int OrderId) : IQuery<OrderDTO>;
}