using BuildingBlocks.Abstractions.Extensions;
using BuildingBlocks.Abstractions.Repository;
using BuildingBlocks.CQRS;
using BuildingBlocks.Error;
using BuildingBlocks.Results;
using ProductService.Domain.Abstractions.Repositories;
using ProductService.Domain.Entities;

namespace ProductService.Application.Commands.DeleteImages
{
    public class DeleteImageCommandHandler : ICommandHandler<DeleteImageCommand>
    {
        private readonly IFileService _fileService;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteImageCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork, IFileService fileService)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
            _fileService = fileService;
        }

        public async Task<Result> Handle(DeleteImageCommand request, CancellationToken cancellationToken)
        {
            Product? product = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken);
            if (product == null)
            {
                Error error = new("ProductNotFound", $"Product with ID {request.ProductId} does not exist.", ErrorType.NotFound);
                return Result.Failure(error);
            }

            List<string> imagesToRemove = product.ProductImages.Where(img => request.ImageIds.Contains(img.Id)).Select(img => img.ImageUrl).ToList();

            foreach (string image in imagesToRemove)
            {
                bool deleted = await _fileService.DeleteFile(image);
                if (!deleted)
                {
                    Error error = new("FailToDelete", "Cant Delete Images", ErrorType.Problem);
                    return Result.Failure(error);
                }
            }

            product.RemoveProductImages(request.ImageIds);

            _productRepository.Update(product, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(true);
        }
    }
}