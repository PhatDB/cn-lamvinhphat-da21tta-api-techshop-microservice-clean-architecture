using BuildingBlocks.CQRS;
using BuildingBlocks.Error;
using BuildingBlocks.Results;
using ProductService.Domain.Abstractions.Repositories;
using ProductService.Domain.Entities;

namespace ProductService.Application.Queries
{
    public class
        GetProductDetailQueryHandler : IQueryHandler<GetProductDetailQuery, Product>
    {
        private readonly IProductRepository _productRepository;

        public GetProductDetailQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Result<Product>> Handle(
            GetProductDetailQuery request, CancellationToken cancellationToken)
        {
            Result<Product> productResult =
                await _productRepository.GetProductDetailAsync(request.ProductId);

            if (productResult.IsFailure)
                return Result.Failure<Product>(new Error("ProductNotFound",
                    $"Product with ID {request.ProductId} does not exist.",
                    ErrorType.NotFound));

            Product product = productResult.Value;
            return Result.Success(product);
        }
    }
}