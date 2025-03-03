using BuildingBlocks.Abstractions.Repository;
using BuildingBlocks.Results;
using ProductService.Domain.Entities;

namespace ProductService.Domain.Abstractions.Repositories
{
    public interface IColorRepository : IGenericRepository<Color>
    {
        Task<Result<Color>> GetColorByNameAsync(string colorName, CancellationToken cancellationToken);
    }
}