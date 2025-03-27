using OrderService.Domain.Abstractions.Repositories;
using OrderService.Domain.Entities;

namespace OrderService.Persistence.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}