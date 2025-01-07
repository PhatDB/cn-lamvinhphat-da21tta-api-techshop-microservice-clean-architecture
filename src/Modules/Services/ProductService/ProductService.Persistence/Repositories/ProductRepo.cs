using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ProductService.Domain.Abstractions.Repositories;
using ProductService.Domain.Entities;

namespace ProductService.Persistence.Repositories
{
    public class ProductRepo : IProductRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Product> _dbSet;

        public ProductRepo(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = _context.Set<Product>();
        }

        public async Task<int> AddAsync(Product Product, CancellationToken cancellationToken)
        {
            if (Product == null) throw new ArgumentNullException(nameof(Product));
            await _dbSet.AddAsync(Product, cancellationToken);
            return Product.Id;
        }

        public async Task<List<Product>> AddRangeAsync(List<Product> products, CancellationToken cancellationToken)
        {
            if (products == null || !products.Any()) throw new ArgumentNullException(nameof(products));
            await _dbSet.AddRangeAsync(products, cancellationToken);
            return await _context.SaveChangesAsync(cancellationToken) > 0 ? products : new List<Product>();
        }

        public async Task<Product> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            Product? Product = await _dbSet.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            if (Product != null) await _dbSet.Entry(Product).Collection(c => c.ProductImages).LoadAsync(cancellationToken);
            return Product;
        }

        public async Task<List<Product>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _dbSet.Include(c => c.ProductImages).ToListAsync(cancellationToken);
        }

        public async Task<bool> IsExistAsync(int id, CancellationToken cancellationToken)
        {
            return await _dbSet.AnyAsync(e => e.Id == id, cancellationToken);
        }

        public async Task<Product> GetSingleAsync(Expression<Func<Product, bool>> predicate, CancellationToken cancellationToken)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            return await _dbSet.FirstOrDefaultAsync(predicate, cancellationToken);
        }

        public void Update(Product Product, CancellationToken cancellationToken)
        {
            if (Product == null) throw new ArgumentNullException(nameof(Product));
            _context.Entry(Product).State = EntityState.Modified;
        }

        public void Delete(Product Product, CancellationToken cancellationToken)
        {
            _dbSet.Remove(Product);
        }

        public async Task<int> ExecuteSqlAsync(string sql, CancellationToken cancellationToken, params object[] parameters)
        {
            if (string.IsNullOrWhiteSpace(sql)) throw new ArgumentException("SQL query cannot be null or empty.", nameof(sql));
            return await _context.Database.ExecuteSqlRawAsync(sql, parameters, cancellationToken);
        }
    }
}