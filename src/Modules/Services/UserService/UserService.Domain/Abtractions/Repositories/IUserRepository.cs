using BuildingBlocks.Abstractions.Repository;
using UserService.Domain.Entities;

namespace UserService.Domain.Abtractions.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
    }
}