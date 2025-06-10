using AutoMapper;
using AutoMapper.QueryableExtensions;
using BuildingBlocks.Contracts.Customers;
using BuildingBlocks.CQRS;
using BuildingBlocks.Extensions;
using BuildingBlocks.Results;
using Microsoft.EntityFrameworkCore;
using ProductService.Application.Abtractions;
using ProductService.Application.DTOs;
using ProductService.Application.Extensions;
using ProductService.Domain.Abstractions.Repositories;
using ProductService.Domain.Entities;

namespace ProductService.Application.Queries.Products.GetActiveProductFilter
{
    public class
        GetActiveProductFilterQueryHandler : IQueryHandler<GetActiveProductFilterQuery, PagedResult<GetAllProductDTO>>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;
        private readonly IProductService _productService;

        public GetActiveProductFilterQueryHandler(
            ICategoryRepository categoryRepository, IMapper mapper, IProductRepository productRepository,
            IProductService productService)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _productRepository = productRepository;
            _productService = productService;
        }

        public async Task<Result<PagedResult<GetAllProductDTO>>> Handle(
            GetActiveProductFilterQuery request, CancellationToken cancellationToken)
        {
            string keyword = request.Keyword?.ToLower() ?? string.Empty;

            IQueryable<Product> query = _productRepository.AsQueryable().Include(p => p.ProductImages)
                .Include(p => p.ProductSpecs).Where(p => p.IsActive).AsNoTracking();

            if (!string.IsNullOrWhiteSpace(keyword))
                query = query.Where(p => p.ProductName.ToLower().Contains(keyword));

            if (request.CategoryId.HasValue)
            {
                List<int> relativeCategoryId = await _categoryRepository.AsQueryable().AsNoTracking()
                    .Where(c => c.ParentId == request.CategoryId).Select(c => c.Id).ToListAsync(cancellationToken);

                relativeCategoryId.Add(request.CategoryId.Value);
                query = query.Where(p => relativeCategoryId.Contains(p.CategoryId));
            }

            if (request.BrandId.HasValue)
                query = query.Where(p => p.BrandId == request.BrandId.Value);

            if (request.MinPrice.HasValue)
                query = query.Where(p => p.Price >= request.MinPrice.Value);

            if (request.MaxPrice.HasValue)
                query = query.Where(p => p.Price <= request.MaxPrice.Value);

            if (request.IsFeatured.HasValue)
                query = query.Where(p => p.IsFeatured == request.IsFeatured.Value);

            int totalCount = await query.CountAsync(cancellationToken);

            IQueryable<Product> sorted = query.ApplySorting(request.PaginationOption);

            List<GetAllProductDTO> items = await sorted
                .Skip((request.PaginationOption.PageNumber!.Value - 1) * request.PaginationOption.PageSize!.Value)
                .Take(request.PaginationOption.PageSize.Value)
                .ProjectTo<GetAllProductDTO>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken);

            List<int> productIds = items.Select(p => p.Id).ToList();
            Result<GetProductsRatingResponse> ratingResult = await _productService.GetProductsRating(productIds);

            if (ratingResult.IsSuccess)
            {
                Dictionary<int, byte> ratingMap =
                    ratingResult.Value.Ratings.ToDictionary(r => r.ProductId, r => r.Rating);
                foreach (GetAllProductDTO? item in items)
                    if (ratingMap.TryGetValue(item.Id, out byte rating))
                        item.Rating = rating;
            }

            return Result.Success(new PagedResult<GetAllProductDTO>(items, totalCount,
                request.PaginationOption.PageNumber.Value, request.PaginationOption.PageSize.Value));
        }
    }
}