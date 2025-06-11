using BuildingBlocks.CQRS;
using OrderService.Application.DTOs;

namespace OrderService.Application.Queries.Orders.GetAllOrder
{
    public record GetAllOrderQuery : IQuery<List<OrderDTO>>;
}