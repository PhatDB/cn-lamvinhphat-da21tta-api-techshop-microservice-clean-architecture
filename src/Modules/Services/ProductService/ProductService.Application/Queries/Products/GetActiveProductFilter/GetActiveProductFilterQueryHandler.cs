using AutoMapper;
using AutoMapper.QueryableExtensions;
using BuildingBlocks.CQRS;
using BuildingBlocks.Extensions;
using BuildingBlocks.Results;
using Microsoft.EntityFrameworkCore;
using ProductService.Application.DTOs;
using ProductService.Application.Extensions;
using ProductService.Domain.Abstractions.Repositories;
using ProductService.Domain.Entities;

namespace ProductService.Application.Queries.Products.GetActiveProductFilter
{
    public class
        GetActiveProductFilterQueryHandler : IQueryHandler<GetActiveProductFilterQuery, PagedResult<GetAllProductDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;

        public GetActiveProductFilterQueryHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<Result<PagedResult<GetAllProductDTO>>> Handle(
            GetActiveProductFilterQuery request, CancellationToken cancellationToken)
        {
            string keyword = request.Keyword?.ToLower() ?? string.Empty;

            IQueryable<Product>? query = _productRepository.AsQueryable().Include(p => p.ProductImages)
                .Where(p => p.IsActive).AsNoTracking();

            if (!string.IsNullOrWhiteSpace(keyword))
                query = query.Where(p => p.ProductName.ToLower().Contains(keyword));

            if (request.CategoryId.HasValue) query = query.Where(p => p.CategoryId == request.CategoryId.Value);

            if (request.BrandId.HasValue) query = query.Where(p => p.BrandId == request.BrandId.Value);

            if (request.MinPrice.HasValue) query = query.Where(p => p.Price >= request.MinPrice.Value);

            if (request.MaxPrice.HasValue) query = query.Where(p => p.Price <= request.MaxPrice.Value);

            if (request.IsFeatured.HasValue) query = query.Where(p => p.IsFeatured == request.IsFeatured.Value);

            int totalCount = await query.CountAsync(cancellationToken);

            IQueryable<Product> sorted = query.ApplySorting(request.PaginationOption);

            List<GetAllProductDTO> items = await sorted
                .Skip((request.PaginationOption.PageNumber!.Value - 1) * request.PaginationOption.PageSize!.Value)
                .Take(request.PaginationOption.PageSize.Value)
                .ProjectTo<GetAllProductDTO>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken);

            return new PagedResult<GetAllProductDTO>(items, totalCount, request.PaginationOption.PageNumber.Value,
                request.PaginationOption.PageSize.Value);
        }
    }
}