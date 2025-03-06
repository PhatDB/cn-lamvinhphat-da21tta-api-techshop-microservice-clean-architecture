using AutoMapper;
using BuildingBlocks.CQRS;
using BuildingBlocks.Error;
using BuildingBlocks.Results;
using Microsoft.EntityFrameworkCore;
using ProductService.Application.DTOs;
using ProductService.Domain.Abstractions.Repositories;
using ProductService.Domain.Entities;

namespace ProductService.Application.Queries
{
    public class
        GetProductDetailQueryHandler : IQueryHandler<GetProductDetailQuery,
        ProductDetailDTO>
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;

        public GetProductDetailQueryHandler(
            IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<Result<ProductDetailDTO>> Handle(
            GetProductDetailQuery request, CancellationToken cancellationToken)
        {
            Product? product = await _productRepository.AsQueryable()
                .Include(p => p.ProductImages).Include(p => p.Inventory)
                .FirstOrDefaultAsync(p => p.Id == request.ProductId, cancellationToken);

            if (product == null)
                return Result.Failure<ProductDetailDTO>(Error.NotFound("Product.NotFound",
                    $"Product with ID {request.ProductId} not found."));

            return Result.Success(_mapper.Map<ProductDetailDTO>(product));
        }
    }
}