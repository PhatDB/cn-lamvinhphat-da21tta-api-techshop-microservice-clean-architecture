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

namespace ProductService.Application.Queries.Products.GetActiveProductByCategoryId
{
    public class
        GetActiveProductByCategoryIdQueryHandler : IQueryHandler<GetActiveProductByCategoryIdQuery,
        PagedResult<GetAllProductDTO>>
    {
        private readonly ICategoryRepository _categoryRepository;

        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;

        public GetActiveProductByCategoryIdQueryHandler(
            IMapper mapper, IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _mapper = mapper;
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }


        public async Task<Result<PagedResult<GetAllProductDTO>>> Handle(
            GetActiveProductByCategoryIdQuery request, CancellationToken cancellationToken)
        {
            List<int> relativeCategoryId = await _categoryRepository.AsQueryable().AsNoTracking()
                .Where(c => c.ParentId == request.CategoryId).Select(c => c.Id).ToListAsync(cancellationToken);

            relativeCategoryId.Add(request.CategoryId);

            IQueryable<Product> query = _productRepository.AsQueryable().Include(p => p.ProductImages)
                .Include(p => p.ProductSpecs).Where(p => relativeCategoryId.Contains(p.CategoryId) && p.IsActive)
                .AsNoTracking();

            int totalCount = await query.CountAsync(cancellationToken);

            IQueryable<Product> sorted = query.ApplySorting(request.PaginationOption);

            List<GetAllProductDTO> items = await sorted
                .Skip((request.PaginationOption.PageNumber!.Value - 1) * request.PaginationOption.PageSize!.Value)
                .Take(request.PaginationOption.PageSize.Value)
                .ProjectTo<GetAllProductDTO>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken);

            return new PagedResult<GetAllProductDTO>(items, totalCount, request.PaginationOption.PageNumber,
                request.PaginationOption.PageSize);
        }
    }
}