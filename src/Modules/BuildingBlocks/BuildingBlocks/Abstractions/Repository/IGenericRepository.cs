using System.Linq.Expressions;
using BuildingBlocks.Abstractions.Aggregates;
using BuildingBlocks.Abstractions.Entities;
using BuildingBlocks.Results;

namespace BuildingBlocks.Abstractions.Repository
{
    public interface IGenericRepository<T> where T : Entity, IAggregateRoot
    {
        Task<Result<int>> AddAsync(T aggregateRoot, CancellationToken cancellationToken = default);
        Task<Result> AddRangeAsync(List<T> aggregateRoots, CancellationToken cancellationToken = default);
        Task<Result<T>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<Result<List<T>>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Result<bool>> IsExistAsync(int id, CancellationToken cancellationToken = default);
        Task<Result<T>> GetSingleAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
        Task<Result> UpdateAsync(T aggregateRoot, CancellationToken cancellationToken = default);
        Task<Result> DeleteAsync(T aggregateRoot, CancellationToken cancellationToken = default);
        Task<Result<int>> ExecuteSqlAsync(string sql, CancellationToken cancellationToken = default, params object[] parameters);
        IQueryable<T> AsQueryable();
    }
}