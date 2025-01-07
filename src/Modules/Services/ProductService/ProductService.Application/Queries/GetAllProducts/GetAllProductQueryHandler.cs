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

            query = ApplySorting(query, request.PaginationOption.SortBy, request.PaginationOption.IsDescending.Value);

            int totalCount = await query.CountAsync(cancellationToken);

            List<GetAllProductDTO> pagedProducts = await query.Skip((request.PaginationOption.PageNumber.Value - 1) * request.PaginationOption.PageSize.Value)
                .Take(request.PaginationOption.PageSize.Value).AsNoTracking().Select(p => new GetAllProductDTO
                {
                    Id = p.Id,
                    ProductName = p.ProductName,
                    Description = p.Description,
                    Price = p.Price,
                    DiscountPrice = p.DiscountPrice,
                    IsActive = p.IsActive,
                    FirstImageUrl = p.ProductImages.FirstOrDefault().ImageUrl
                }).ToListAsync(cancellationToken);

            PagedResult<GetAllProductDTO> pagedResult = new(pagedProducts, totalCount, request.PaginationOption.PageNumber.Value, request.PaginationOption.PageSize.Value);

            return Result.Success(pagedResult);
        }

        private IQueryable<Product> ApplySorting(IQueryable<Product> query, string sortBy, bool isDescending)
        {
            switch (sortBy.ToLower())
            {
                case "":
                    query = isDescending ? query.OrderByDescending(p => p.CreatedDate) : query.OrderBy(p => p.CreatedDate);
                    break;
                case "price":
                    query = isDescending ? query.OrderByDescending(p => p.Price) : query.OrderBy(p => p.Price);
                    break;
                case "productname":
                    query = isDescending ? query.OrderByDescending(p => p.ProductName) : query.OrderBy(p => p.ProductName);
                    break;
                default:
                    query = query.OrderBy(p => p.ProductName);
                    break;
            }

            return query;
        }
    }
}