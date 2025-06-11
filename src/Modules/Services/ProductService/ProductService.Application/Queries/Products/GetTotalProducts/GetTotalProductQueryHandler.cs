using BuildingBlocks.CQRS;
using BuildingBlocks.Results;
using Microsoft.EntityFrameworkCore;
using ProductService.Application.DTOs;
using ProductService.Domain.Abstractions.Repositories;

namespace ProductService.Application.Queries.Products.GetTotalProducts
{
    public class GetTotalProductQueryHandler : IQueryHandler<GetTotalProductsQuery, TotalProductDto>
    {
        private readonly IProductRepository _productRepository;

        public GetTotalProductQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Result<TotalProductDto>> Handle(
            GetTotalProductsQuery request, CancellationToken cancellationToken)
        {
            int result = await _productRepository.AsQueryable().CountAsync(cancellationToken);
            TotalProductDto totalProductDto = new();
            totalProductDto.totalProduct = result;
            return Result.Success(totalProductDto);
        }
    }
}