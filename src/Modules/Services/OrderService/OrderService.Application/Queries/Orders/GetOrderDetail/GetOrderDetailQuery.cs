using BuildingBlocks.CQRS;
using OrderService.Application.DTOs;

namespace OrderService.Application.Queries.Orders.GetOrderDetail
{
    public record GetOrderDetailQuery(int OrderId) : IQuery<OrderDTO>;
}