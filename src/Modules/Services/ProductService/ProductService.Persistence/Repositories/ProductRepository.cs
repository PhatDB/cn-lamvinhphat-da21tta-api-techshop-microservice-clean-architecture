using System.Linq.Expressions;
using BuildingBlocks.Error;
using BuildingBlocks.Extensions;
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

        public async Task<Result<PagedResult<Product>>> GetAllPagedAsync(
            PaginationOption paginationOption, CancellationToken cancellationToken)
        {
            IQueryable<Product> query = _context.Products.Include(p => p.ProductImages)
                .Include(p => p.ProductColors).ThenInclude(pc => pc.Color).AsNoTracking();

            Dictionary<string, Expression<Func<Product, object>>> sortingColumns = new()
            {
                { "name", p => p.Name },
                { "price", p => p.Price },
                { "discountprice", p => p.DiscountPrice ?? 0 },
                { "createdat", p => p.CreatedAt },
                { "soldquantity", p => p.SoldQuantity },
                { "categoryid", p => p.CategoryId }
            };

            string sortBy = paginationOption.SortBy?.ToLower() ?? "createdat";
            bool isDescending = paginationOption.IsDescending ?? false;
            int pageNumber = Math.Max(paginationOption.PageNumber ?? 1, 1);
            int pageSize = Math.Clamp(paginationOption.PageSize ?? 10, 1, 100);

            if (!sortingColumns.ContainsKey(sortBy))
                sortBy = "createdat";

            Expression<Func<Product, object>> sortExpression = sortingColumns[sortBy];

            query = isDescending
                ? query.OrderByDescending(sortExpression)
                : query.OrderBy(sortExpression);

            int totalCount = await _context.Products.CountAsync(cancellationToken);

            List<Product> products = await query.Skip((pageNumber - 1) * pageSize)
                .Take(pageSize).ToListAsync(cancellationToken);

            return Result.Success(new PagedResult<Product>(products, totalCount,
                pageNumber, pageSize));
        }

        public async Task<Result<Product>> GetProductDetailAsync(
            int id, CancellationToken cancellationToken = default)
        {
            Product? product = await _context.Products.Include(p => p.ProductImages)
                .Include(p => p.ProductColors).ThenInclude(pc => pc.Color)
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

            return product is null
                ? Result.Failure<Product>(Error.NotFound("Product.NotFound",
                    $"Product with ID {id} not found."))
                : Result.Success(product);
        }
    }
}