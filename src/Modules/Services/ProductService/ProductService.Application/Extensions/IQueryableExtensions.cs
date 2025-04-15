using BuildingBlocks.Extensions;
using ProductService.Domain.Entities;

namespace ProductService.Application.Extensions
{
    public static class IQueryableExtensions
    {
        public static IQueryable<Product> ApplySorting(this IQueryable<Product> query, PaginationOption option)
        {
            if (string.IsNullOrWhiteSpace(option.SortBy))
                return query.OrderByDescending(p => p.CreatedAt);

            string sortBy = option.SortBy.ToLower().Trim();
            bool isDescending = option.IsDescending ?? false;

            return sortBy switch
            {
                "price" => isDescending ? query.OrderByDescending(p => p.Price) : query.OrderBy(p => p.Price),
                "name" or "productname" => isDescending
                    ? query.OrderByDescending(p => p.ProductName)
                    : query.OrderBy(p => p.ProductName),
                "sold" or "sold_quantity" => isDescending
                    ? query.OrderByDescending(p => p.SoldQuantity)
                    : query.OrderBy(p => p.SoldQuantity),
                "created" or "created_at" => isDescending
                    ? query.OrderByDescending(p => p.CreatedAt)
                    : query.OrderBy(p => p.CreatedAt),
                _ => query.OrderByDescending(p => p.CreatedAt)
            };
        }
    }
}