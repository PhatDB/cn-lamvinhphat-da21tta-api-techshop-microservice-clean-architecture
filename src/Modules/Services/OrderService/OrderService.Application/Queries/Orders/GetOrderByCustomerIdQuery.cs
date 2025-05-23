using BuildingBlocks.CQRS;
using OrderService.Application.DTOs;

namespace OrderService.Application.Queries.Orders
{
    public record GetOrderByCustomerIdQuery(int CustomerId) : IQuery<OrderDTO>;
}