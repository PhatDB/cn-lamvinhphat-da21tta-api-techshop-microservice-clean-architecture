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

namespace ProductService.Application.Queries.Products.GetAllActiveProducts
{
    public class
        GetAllActiveProductQueryHandler : IQueryHandler<GetAllActiveProductQuery, PagedResult<GetAllProductDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;

        public GetAllActiveProductQueryHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<Result<PagedResult<GetAllProductDTO>>> Handle(
            GetAllActiveProductQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Product> query = _productRepository.AsQueryable().Include(p => p.ProductImages)
                .Include(p => p.ProductSpecs).Where(p => p.IsActive).AsNoTracking();

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