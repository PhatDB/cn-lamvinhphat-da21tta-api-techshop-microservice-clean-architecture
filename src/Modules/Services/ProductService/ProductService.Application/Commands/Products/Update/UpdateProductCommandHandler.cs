using BuildingBlocks.Abstractions.Repository;
using BuildingBlocks.CQRS;
using BuildingBlocks.Results;
using ProductService.Domain.Abstractions.Repositories;
using ProductService.Domain.Entities;

namespace ProductService.Application.Commands.Products.Update
{
    public class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand>
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProductCommandHandler(
            IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(
            UpdateProductCommand request, CancellationToken cancellationToken)
        {
            Result<Product> productResult =
                await _productRepository.GetByIdAsync(request.ProductId,
                    cancellationToken);

            if (productResult.IsFailure)
                return Result.Failure(productResult.Error);

            Product product = productResult.Value;

            product.UpdateProduct(request.Name, request.Sku, request.Price,
                request.CategoryId, request.Description, request.DiscountPrice);

            await _productRepository.UpdateAsync(product, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}