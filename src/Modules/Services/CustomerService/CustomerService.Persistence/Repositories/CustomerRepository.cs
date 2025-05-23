using CustomerService.Domain.Abtractions.Repositories;
using CustomerService.Domain.Entities;

namespace CustomerService.Persistence.Repositories
{
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        private readonly ApplicationDbContext _context;

        public CustomerRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}