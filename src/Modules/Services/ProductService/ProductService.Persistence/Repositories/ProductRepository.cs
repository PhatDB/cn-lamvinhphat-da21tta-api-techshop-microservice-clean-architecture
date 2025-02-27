using Microsoft.EntityFrameworkCore;
using ProductService.Domain.Abstractions.Repositories;
using ProductService.Domain.Entities;
using BuildingBlocks.Results;
using BuildingBlocks.Error;

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
            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(sortBy))
            {
                query = isDescending == true
                    ? query.OrderByDescending(p => EF.Property<object>(p, sortBy))
                    : query.OrderBy(p => EF.Property<object>(p, sortBy));
            }

            return await query.Skip((pageNumber - 1) * pageSize)
                              .Take(pageSize)
                              .ToListAsync(cancellationToken);
        }

        public async Task<int> GetTotalCountAsync(CancellationToken cancellationToken)
        {
            return await _context.Products.CountAsync(cancellationToken);
        }

        public async Task<Result<Color>> GetColorByNameAsync(string colorName, CancellationToken cancellationToken)
        {
            var color = await _context.Colors.FirstOrDefaultAsync(c => c.Name == colorName, cancellationToken);

            return color is null
                ? Result.Failure<Color>(Error.NotFound("Color.NotFound", $"Color '{colorName}' not found."))
                : Result.Success(color);
        }

        public async Task<Result> AddColorAsync(Color color, CancellationToken cancellationToken)
        {
            await _context.Colors.AddAsync(color, cancellationToken);
            return Result.Success();
        }
    }
}
