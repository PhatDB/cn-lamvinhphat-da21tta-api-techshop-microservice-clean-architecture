using System.Linq.Expressions;
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
            IQueryable<Product> query = _context.Products.Include(p => p.ProductImages);

            Dictionary<string, Expression<Func<Product, object>>> sortingColumns = new()
            {
                { "product_name ", p => p.ProductName },
                { "price", p => p.Price },
                { "discount ", p => p.Discount },
                { "created_at ", p => p.CreatedAt },
                { "sold_quantity ", p => p.SoldQuantity },
                { "category_id ", p => p.CategoryId },
                { "brand_id  ", p => p.BrandId }
            };

            string sortBy = paginationOption.SortBy?.ToLower() ?? "created_at";
            bool isDescending = paginationOption.IsDescending ?? false;
            int pageNumber = Math.Max(paginationOption.PageNumber ?? 1, 1);
            int pageSize = Math.Clamp(paginationOption.PageSize ?? 10, 1, 100);

            if (!sortingColumns.ContainsKey(sortBy))
                sortBy = "created_at";

            Expression<Func<Product, object>> sortExpression = sortingColumns[sortBy];

            query = isDescending ? query.OrderByDescending(sortExpression) : query.OrderBy(sortExpression);

            int totalCount = await _context.Products.CountAsync(cancellationToken);

            List<Product> products = await query.Skip((pageNumber - 1) * pageSize)
                .Take(pageSize).ToListAsync(cancellationToken);

            return Result.Success(new PagedResult<Product>(products, totalCount, pageNumber, pageSize));
        }

        public async Task<Result<PagedResult<Product>>> GetAllProductByCategoryIdPagedAsync(
            PaginationOption paginationOption, int categoryId, CancellationToken cancellationToken)
        {
            IQueryable<Product> query = _context.Products.Include(p => p.ProductImages)
                .Where(p => p.CategoryId == categoryId).AsNoTracking();

            Dictionary<string, Expression<Func<Product, object>>> sortingColumns = new()
            {
                { "product_name ", p => p.ProductName },
                { "price", p => p.Price },
                { "discount ", p => p.Discount },
                { "created_at ", p => p.CreatedAt },
                { "sold_quantity ", p => p.SoldQuantity },
                { "category_id ", p => p.CategoryId },
                { "brand_id  ", p => p.BrandId }
            };

            string sortBy = paginationOption.SortBy?.ToLower() ?? "created_at";
            bool isDescending = paginationOption.IsDescending ?? false;
            int pageNumber = Math.Max(paginationOption.PageNumber ?? 1, 1);
            int pageSize = Math.Clamp(paginationOption.PageSize ?? 10, 1, 100);

            if (!sortingColumns.ContainsKey(sortBy))
                sortBy = "created_at";

            Expression<Func<Product, object>> sortExpression = sortingColumns[sortBy];

            query = isDescending ? query.OrderByDescending(sortExpression) : query.OrderBy(sortExpression);

            int totalCount = await _context.Products.CountAsync(cancellationToken);

            List<Product> products = await query.Skip((pageNumber - 1) * pageSize)
                .Take(pageSize).ToListAsync(cancellationToken);

            return Result.Success(new PagedResult<Product>(products, totalCount, pageNumber, pageSize));
        }
    }
}