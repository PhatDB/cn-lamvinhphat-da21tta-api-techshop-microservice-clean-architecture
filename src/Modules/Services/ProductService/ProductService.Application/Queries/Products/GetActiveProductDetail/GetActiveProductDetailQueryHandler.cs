using AutoMapper;
using BuildingBlocks.CQRS;
using BuildingBlocks.Results;
using Microsoft.EntityFrameworkCore;
using ProductService.Application.DTOs;
using ProductService.Domain.Abstractions.Repositories;
using ProductService.Domain.Entities;
using ProductService.Domain.Errors;

namespace ProductService.Application.Queries.Products.GetActiveProductDetail
{
    public class GetActiveProductDetailQueryHandler : IQueryHandler<GetActiveProductDetailQuery, ProductDetailDTO>
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;

        public GetActiveProductDetailQueryHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<Result<ProductDetailDTO>> Handle(
            GetActiveProductDetailQuery request, CancellationToken cancellationToken)
        {
            Product? product = await _productRepository.AsQueryable().Include(p => p.ProductImages)
                .Where(p => p.IsActive).AsNoTracking()
                .SingleOrDefaultAsync(p => p.Id == request.ProductId, cancellationToken);

            return product != null
                ? _mapper.Map<ProductDetailDTO>(product)
                : Result.Failure<ProductDetailDTO>(ProductError.ProductNotFound);
        }
    }
}