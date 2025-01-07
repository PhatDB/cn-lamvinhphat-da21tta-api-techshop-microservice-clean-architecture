using BuildingBlocks.CQRS;
using BuildingBlocks.Error;
using BuildingBlocks.Results;
using Microsoft.EntityFrameworkCore;
using ProductService.Domain.Abstractions.Repositories;
using ProductService.Domain.Entities;

namespace ProductService.Application.Queries
{
    public class GetProductDetailQueryHandler : IQueryHandler<GetProductDetailQuery, Product>
    {
        private readonly IProductRepository _productRepository;

        public GetProductDetailQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Result<Product>> Handle(GetProductDetailQuery request, CancellationToken cancellationToken)
        {
            Product? product = await _productRepository.AsQueryable().Where(p => p.Id == request.ProductId).Include(p => p.ProductImages).FirstOrDefaultAsync(cancellationToken);

            if (product == null) return Result.Failure<Product>(new Error("ProductNotFound", $"Product with ID {request.ProductId} does not exist.", ErrorType.NotFound));

            return Result.Success(product);
        }
    }
}