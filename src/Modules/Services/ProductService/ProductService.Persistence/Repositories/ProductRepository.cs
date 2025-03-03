using BuildingBlocks.Error;
using BuildingBlocks.Results;
using Microsoft.EntityFrameworkCore;
using ProductService.Domain.Abstractions.Repositories;
using ProductService.Domain.Entities;

namespace ProductService.Persistence.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetAllPagedAsync(int pageNumber, int pageSize, string? sortBy, bool? isDescending, CancellationToken cancellationToken)
        {
            IQueryable<Product> query = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(sortBy))
                query = isDescending == true ? query.OrderByDescending(p => EF.Property<object>(p, sortBy)) : query.OrderBy(p => EF.Property<object>(p, sortBy));

            return await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
        }

        public async Task<int> GetTotalCountAsync(CancellationToken cancellationToken)
        {
            return await _context.Products.CountAsync(cancellationToken);
        }

        public async Task<Result> AddColorAsync(Color color, CancellationToken cancellationToken)
        {
            await _context.Colors.AddAsync(color, cancellationToken);
            return Result.Success();
        }

        public async Task<Result<Product>> GetProductDetailAsync(int id, CancellationToken cancellationToken = default)
        {
            Product? product = await _context.Products.Include(p => p.ProductImages).Include(p => p.ProductColors).ThenInclude(pc => pc.Color)
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

            return product is null ? Result.Failure<Product>(Error.NotFound("Product.NotFound", $"Product with ID {id} not found.")) : Result.Success(product);
        }
    }
}