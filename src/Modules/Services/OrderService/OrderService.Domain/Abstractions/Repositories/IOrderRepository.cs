using BuildingBlocks.Abstractions.Repository;
using OrderService.Domain.Entities;

namespace OrderService.Domain.Abstractions.Repositories
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
    }
}