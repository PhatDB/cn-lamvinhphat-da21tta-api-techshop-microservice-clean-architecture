using System.Linq.Expressions;
using BuildingBlocks.Abstractions.Aggregates;

namespace BuildingBlocks.Abstractions.Repository
{
    public interface IGenericRepository<T> where T : IAggregateRoot
    {
        Task<int> AddAsync(T aggregateRoot, CancellationToken cancellationToken = default);
        Task<List<T>> AddRangeAsync(List<T> aggregateRoots, CancellationToken cancellationToken = default);
        Task<T> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<bool> IsExistAsync(int id, CancellationToken cancellationToken = default);
        Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
        void Update(T aggregateRoot, CancellationToken cancellationToken = default);
        void Delete(T aggregateRoot, CancellationToken cancellationToken = default);
        Task<int> ExecuteSqlAsync(string sql, CancellationToken cancellationToken = default, params object[] parameters);
    }
}