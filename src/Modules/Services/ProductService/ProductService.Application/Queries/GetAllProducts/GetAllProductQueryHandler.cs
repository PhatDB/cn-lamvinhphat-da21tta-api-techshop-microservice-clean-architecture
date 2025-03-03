using BuildingBlocks.CQRS;
using BuildingBlocks.Extensions;
using BuildingBlocks.Results;
using Microsoft.EntityFrameworkCore;
using ProductService.Application.DTOs;
using ProductService.Domain.Abstractions.Repositories;
using ProductService.Domain.Entities;

namespace ProductService.Application.Queries
{
    public class GetAllProductQueryHandler : IQueryHandler<GetAllProductQuery,
        PagedResult<GetAllProductDTO>>
    {
        private readonly IProductRepository _productRepository;

        public GetAllProductQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Result<PagedResult<GetAllProductDTO>>> Handle(
            GetAllProductQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Product> query = _productRepository.AsQueryable()
                .Include(p => p.ProductImages) // Load danh sách hình ảnh
                .Include(p => p.ProductColors) // Load danh sách màu sắc
                .ThenInclude(pc => pc.Color); // Load chi tiết màu sắc

            string sortBy = string.IsNullOrWhiteSpace(request.PaginationOption.SortBy)
                ? "CreatedAt"
                : request.PaginationOption.SortBy;
            bool isDescending = request.PaginationOption.IsDescending ?? false;

            query = ApplySorting(query, sortBy, isDescending);

            int totalCount = await query.CountAsync(cancellationToken);

            List<GetAllProductDTO> pagedProducts = await query
                .Skip((request.PaginationOption.PageNumber ?? 1 - 1) *
                      (request.PaginationOption.PageSize ?? 10))
                .Take(request.PaginationOption.PageSize ?? 10).AsNoTracking().Select(p =>
                    new GetAllProductDTO
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
                        FirstImageUrl =
                            p.ProductImages.Any()
                                ? p.ProductImages.FirstOrDefault().ImageUrl
                                : null,
                        Colors = p.ProductColors.Select(pc =>
                            new ColorDTO(pc.Color.Name, pc.StockQuantity)).ToList()
                    }).ToListAsync(cancellationToken);

            PagedResult<GetAllProductDTO> pagedResult = new(pagedProducts, totalCount,
                request.PaginationOption.PageNumber ?? 1,
                request.PaginationOption.PageSize ?? 10);

            return Result.Success(pagedResult);
        }

        private IQueryable<Product> ApplySorting(
            IQueryable<Product> query, string sortBy, bool isDescending)
        {
            return sortBy.ToLower() switch
            {
                "createdat" => isDescending
                    ? query.OrderByDescending(p => p.CreatedAt)
                    : query.OrderBy(p => p.CreatedAt),
                "price" => isDescending
                    ? query.OrderByDescending(p => p.Price)
                    : query.OrderBy(p => p.Price),
                "name" => isDescending
                    ? query.OrderByDescending(p => p.Name)
                    : query.OrderBy(p => p.Name),
                _ => query.OrderBy(p => p.CreatedAt)
            };
        }
    }
}