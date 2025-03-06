using AutoMapper;
using BuildingBlocks.CQRS;
using BuildingBlocks.Extensions;
using BuildingBlocks.Results;
using ProductService.Application.DTOs;
using ProductService.Domain.Abstractions.Repositories;
using ProductService.Domain.Entities;

namespace ProductService.Application.Queries.Products.GetProductByCategoryId
{
    public class GetProductByCategoryIdQueryHandler : IQueryHandler<
        GetProductByCategoryIdQuery, PagedResult<GetAllProductDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;

        public GetProductByCategoryIdQueryHandler(
            IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<Result<PagedResult<GetAllProductDTO>>> Handle(
            GetProductByCategoryIdQuery request, CancellationToken cancellationToken)
        {
            PaginationOption paginationOption = request.PaginationOption;

            Result<PagedResult<Product>> pagedProductsResult =
                await _productRepository.GetAllProductByCategoryIdPagedAsync(
                    paginationOption, request.CategoryId, cancellationToken);

            if (pagedProductsResult.IsFailure)
                return Result.Failure<PagedResult<GetAllProductDTO>>(pagedProductsResult
                    .Error);

            PagedResult<Product> pagedProducts = pagedProductsResult.Value;

            List<GetAllProductDTO>? mappedProducts =
                _mapper.Map<List<GetAllProductDTO>>(pagedProducts.Data);

            return Result.Success(new PagedResult<GetAllProductDTO>(mappedProducts,
                pagedProducts.TotalItems, pagedProducts.PageNumber,
                pagedProducts.PageSize));
        }
    }
}