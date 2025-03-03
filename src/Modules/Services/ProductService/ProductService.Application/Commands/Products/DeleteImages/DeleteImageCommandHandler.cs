using BuildingBlocks.Abstractions.Extensions;
using BuildingBlocks.Abstractions.Repository;
using BuildingBlocks.CQRS;
using BuildingBlocks.Error;
using BuildingBlocks.Results;
using ProductService.Domain.Abstractions.Repositories;
using ProductService.Domain.Entities;

namespace ProductService.Application.Commands.Products.DeleteImages
{
    public class DeleteImageCommandHandler : ICommandHandler<DeleteImageCommand>
    {
        private readonly IFileService _fileService;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteImageCommandHandler(
            IProductRepository productRepository, IUnitOfWork unitOfWork,
            IFileService fileService)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
            _fileService = fileService;
        }

        public async Task<Result> Handle(
            DeleteImageCommand request, CancellationToken cancellationToken)
        {
            Result<Product> productResult =
                await _productRepository.GetByIdAsync(request.ProductId,
                    cancellationToken);
            if (productResult.IsFailure)
                return Result.Failure(productResult.Error);

            Product product = productResult.Value;

            List<string> imagesToRemove = product.ProductImages
                .Where(img => request.ImageIds.Contains(img.Id))
                .Select(img => img.ImageUrl).ToList();

            foreach (string image in imagesToRemove)
            {
                bool deleted = await _fileService.DeleteFile(image);
                if (!deleted)
                    return Result.Failure(Error.Problem("FailToDelete",
                        "Cannot delete images."));
            }

            Result removeResult = product.RemoveProductImages(request.ImageIds);
            if (removeResult.IsFailure)
                return Result.Failure(removeResult.Error);

            await _productRepository.UpdateAsync(product, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}