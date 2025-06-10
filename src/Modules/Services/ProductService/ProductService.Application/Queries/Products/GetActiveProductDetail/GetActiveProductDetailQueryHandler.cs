using AutoMapper;
using BuildingBlocks.Contracts.Customers;
using BuildingBlocks.CQRS;
using BuildingBlocks.Results;
using Microsoft.EntityFrameworkCore;
using ProductService.Application.Abtractions;
using ProductService.Application.DTOs;
using ProductService.Domain.Abstractions.Repositories;
using ProductService.Domain.Entities;

namespace ProductService.Application.Queries.Products.GetActiveProductDetail
{
    public class GetActiveProductDetailQueryHandler : IQueryHandler<GetActiveProductDetailQuery, ProductDetailDTO>
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;
        private readonly IProductService _productService;

        public GetActiveProductDetailQueryHandler(
            IMapper mapper, IProductRepository productRepository, IProductService productService)
        {
            _mapper = mapper;
            _productRepository = productRepository;
            _productService = productService;
        }


        public async Task<Result<ProductDetailDTO>> Handle(
            GetActiveProductDetailQuery request, CancellationToken cancellationToken)
        {
            Product? product = await _productRepository.AsQueryable().Include(p => p.ProductImages)
                .Include(p => p.ProductSpecs).AsNoTracking()
                .SingleOrDefaultAsync(p => p.Id == request.ProductId, cancellationToken);


            Result<GetProductReviewsResponse> productReviews =
                await _productService.GetProductReviews(request.ProductId);

            ProductDetailDTO? productDto = _mapper.Map<ProductDetailDTO>(product);

            if (productReviews.IsSuccess)
                productDto.ProductReviews = productReviews.Value.ProductReviews;
            else
                productDto.ProductReviews = new List<ProductReviewDto>();

            return Result.Success(productDto);
        }
    }
}