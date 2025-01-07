using System.Linq.Expressions;
using CatalogService.Domain.Abstractions.Repositories;
using CatalogService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Persistence.Repositories
{
    public class CategoryRepo : ICategoryRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Category> _dbSet;

        public CategoryRepo(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = _context.Set<Category>();
        }

        public async Task<int> AddAsync(Category category, CancellationToken cancellationToken)
        {
            if (category == null) throw new ArgumentNullException(nameof(category));
            await _dbSet.AddAsync(category, cancellationToken);
            return category.Id;
        }

        public async Task<List<Category>> AddRangeAsync(List<Category> categories, CancellationToken cancellationToken)
        {
            if (categories == null || !categories.Any()) throw new ArgumentNullException(nameof(categories));
            await _dbSet.AddRangeAsync(categories, cancellationToken);
            return await _context.SaveChangesAsync(cancellationToken) > 0 ? categories : new List<Category>();
        }

        public async Task<Category> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            Category? category = await _dbSet.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            if (category != null) await _dbSet.Entry(category).Collection(c => c.CategoryItems).LoadAsync(cancellationToken);
            return category;
        }

        public async Task<List<Category>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _dbSet.Include(c => c.CategoryItems).ToListAsync(cancellationToken);
        }

        public async Task<bool> IsExistAsync(int id, CancellationToken cancellationToken)
        {
            return await _dbSet.AnyAsync(e => e.Id == id, cancellationToken);
        }

        public async Task<Category> GetSingleAsync(Expression<Func<Category, bool>> predicate, CancellationToken cancellationToken)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            return await _dbSet.FirstOrDefaultAsync(predicate, cancellationToken);
        }

        public void Update(Category category, CancellationToken cancellationToken)
        {
            if (category == null) throw new ArgumentNullException(nameof(category));
            _context.Entry(category).State = EntityState.Modified;
        }

        public void Delete(Category category, CancellationToken cancellationToken)
        {
            _dbSet.Remove(category);
        }

        public async Task<int> ExecuteSqlAsync(string sql, CancellationToken cancellationToken, params object[] parameters)
        {
            if (string.IsNullOrWhiteSpace(sql)) throw new ArgumentException("SQL query cannot be null or empty.", nameof(sql));
            return await _context.Database.ExecuteSqlRawAsync(sql, parameters, cancellationToken);
        }
    }
}