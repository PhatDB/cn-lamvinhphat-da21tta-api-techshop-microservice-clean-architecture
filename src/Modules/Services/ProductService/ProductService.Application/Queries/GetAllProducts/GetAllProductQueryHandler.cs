using BuildingBlocks.CQRS;
using BuildingBlocks.Extensions;
using BuildingBlocks.Results;
using Microsoft.EntityFrameworkCore;
using ProductService.Application.DTOs;
using ProductService.Domain.Abstractions.Repositories;
using ProductService.Domain.Entities;

namespace ProductService.Application.Queries
{
    public class GetAllProductQueryHandler : IQueryHandler<GetAllProductQuery, PagedResult<GetAllProductDTO>>
    {
        private readonly IProductRepository _productRepository;

        public GetAllProductQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Result<PagedResult<GetAllProductDTO>>> Handle(GetAllProductQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Product> query = _productRepository.AsQueryable();

            // 🔄 Kiểm tra giá trị mặc định
            string sortBy = string.IsNullOrWhiteSpace(request.PaginationOption.SortBy) ? "CreatedAt" : request.PaginationOption.SortBy;
            bool isDescending = request.PaginationOption.IsDescending ?? false;

            query = ApplySorting(query, sortBy, isDescending);

            int totalCount = await query.CountAsync(cancellationToken);

            List<GetAllProductDTO> pagedProducts = await query
                .Skip((request.PaginationOption.PageNumber ?? 1 - 1) * (request.PaginationOption.PageSize ?? 10))
                .Take(request.PaginationOption.PageSize ?? 10)
                .AsNoTracking()
                .Select(p => new GetAllProductDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Sku = p.Sku != null ? p.Sku.Value : string.Empty,
                    Description = p.Description,
                    Price = p.Price,
                    DiscountPrice = p.DiscountPrice,
                    SoldQuantity = p.SoldQuantity,
                    IsActive = p.IsActive,
                    CategoryId = p.CategoryId,
                    FirstImageUrl = p.ProductImages.FirstOrDefault().ImageUrl
                })
                .ToListAsync(cancellationToken);

            PagedResult<GetAllProductDTO> pagedResult = new(pagedProducts, totalCount, request.PaginationOption.PageNumber ?? 1, request.PaginationOption.PageSize ?? 10);

            return Result.Success(pagedResult);
        }

        private IQueryable<Product> ApplySorting(IQueryable<Product> query, string sortBy, bool isDescending)
        {
            switch (sortBy.ToLower())
            {
                case "createdat":
                    query = isDescending ? query.OrderByDescending(p => p.CreatedAt) : query.OrderBy(p => p.CreatedAt);
                    break;
                case "price":
                    query = isDescending ? query.OrderByDescending(p => p.Price) : query.OrderBy(p => p.Price);
                    break;
                case "name":
                    query = isDescending ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name);
                    break;
                default:
                    query = query.OrderBy(p => p.CreatedAt);
                    break;
            }

            return query;
        }
    }
}
