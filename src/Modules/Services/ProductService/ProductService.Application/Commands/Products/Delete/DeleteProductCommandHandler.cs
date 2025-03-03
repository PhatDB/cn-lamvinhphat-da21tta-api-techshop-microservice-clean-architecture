using BuildingBlocks.Abstractions.Repository;
using BuildingBlocks.CQRS;
using BuildingBlocks.Results;
using ProductService.Domain.Abstractions.Repositories;
using ProductService.Domain.Entities;

namespace ProductService.Application.Commands.Products.Delete
{
    public class DeleteProductCommandHandler : ICommandHandler<DeleteProductCommand>
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteProductCommandHandler(
            IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(
            DeleteProductCommand request, CancellationToken cancellationToken)
        {
            Result<Product> productResult =
                await _productRepository.GetByIdAsync(request.ProductId,
                    cancellationToken);
            if (productResult.IsFailure)
                return Result.Failure(productResult.Error);

            Product product = productResult.Value;

            Result deleteResult = product.DeleteProduct();
            if (deleteResult.IsFailure)
                return Result.Failure(deleteResult.Error);

            await _productRepository.UpdateAsync(product, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}