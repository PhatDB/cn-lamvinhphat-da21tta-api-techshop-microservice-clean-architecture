using System.Linq.Expressions;
using BuildingBlocks.Abstractions.Aggregates;

namespace BuildingBlocks.Abstractions.Repository
{
    public interface IGenericRepository<T> where T : IAggregateRoot
    {
        Task<int> AddAsync(T aggregateRoot, CancellationToken cancellationToken);
        Task<List<T>> AddRangeAsync(List<T> aggregateRoots, CancellationToken cancellationToken);
        Task<T> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<List<T>> GetAllAsync(List<int> ids, CancellationToken cancellationToken);
        Task<bool> IsExistAsync(int id, CancellationToken cancellationToken);
        Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken);
        Task<int> UpdateAsync(T aggregateRoot, CancellationToken cancellationToken);
        Task<int> DeleteAsync(int id, CancellationToken cancellationToken);
        Task<int> ExecuteSqlAsync(string sql, CancellationToken cancellationToken, params object[] parameters);
    }
}