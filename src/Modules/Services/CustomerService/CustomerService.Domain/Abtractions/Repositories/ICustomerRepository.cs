using BuildingBlocks.Abstractions.Repository;
using CustomerService.Domain.Entities;

namespace CustomerService.Domain.Abtractions.Repositories
{
    public interface ICustomerRepository : IGenericRepository<Customer>
    {
    }
}