using System.Linq.Expressions;
using BuildingBlocks.Abstractions.Aggregates;
using BuildingBlocks.Abstractions.Entities;
using BuildingBlocks.Abstractions.Repository;
using BuildingBlocks.Error;
using BuildingBlocks.Results;
using Microsoft.EntityFrameworkCore;

namespace ProductService.Persistence.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T>
        where T : Entity, IAggregateRoot
    {
        private readonly DbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(DbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<Result<int>> AddAsync(
            T aggregateRoot, CancellationToken cancellationToken = default)
        {
            await _dbSet.AddAsync(aggregateRoot, cancellationToken);
            return Result.Success(aggregateRoot.Id);
        }

        public async Task<Result> AddRangeAsync(
            List<T> aggregateRoots, CancellationToken cancellationToken = default)
        {
            await _dbSet.AddRangeAsync(aggregateRoots, cancellationToken);
            return Result.Success();
        }

        public async Task<Result<T>> GetByIdAsync(
            int id, CancellationToken cancellationToken = default)
        {
            T? entity = await _dbSet.FindAsync(new object[] { id }, cancellationToken);

            return entity is null
                ? Result.Failure<T>(Error.NotFound("Entity.NotFound",
                    $"{typeof(T).Name} with ID {id} not found."))
                : Result.Success(entity);
        }

        public async Task<Result<List<T>>> GetAllAsync(
            CancellationToken cancellationToken = default)
        {
            List<T> entities = await _dbSet.AsNoTracking().ToListAsync(cancellationToken);
            return Result.Success(entities);
        }

        public async Task<Result<bool>> IsExistAsync(
            int id, CancellationToken cancellationToken = default)
        {
            bool exists = await _dbSet.AnyAsync(e => EF.Property<int>(e, "Id") == id,
                cancellationToken);
            return Result.Success(exists);
        }

        public async Task<Result<T>> GetSingleAsync(
            Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default)
        {
            T? entity = await _dbSet.AsNoTracking()
                .FirstOrDefaultAsync(predicate, cancellationToken);

            return entity is null
                ? Result.Failure<T>(Error.NotFound("Entity.NotFound",
                    $"{typeof(T).Name} not found with specified condition."))
                : Result.Success(entity);
        }

        public async Task<Result> UpdateAsync(
            T aggregateRoot, CancellationToken cancellationToken = default)
        {
            _dbSet.Update(aggregateRoot);
            return Result.Success();
        }

        public async Task<Result> DeleteAsync(
            T aggregateRoot, CancellationToken cancellationToken = default)
        {
            _dbSet.Remove(aggregateRoot);
            return Result.Success();
        }

        public async Task<Result<int>> ExecuteSqlAsync(
            string sql, CancellationToken cancellationToken = default,
            params object[] parameters)
        {
            int result =
                await _context.Database.ExecuteSqlRawAsync(sql, parameters,
                    cancellationToken);
            return Result.Success(result);
        }

        public IQueryable<T> AsQueryable()
        {
            return _dbSet.AsQueryable();
        }
    }
}