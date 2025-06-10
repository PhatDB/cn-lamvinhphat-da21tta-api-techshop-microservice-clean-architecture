using BuildingBlocks.Abstractions.Extensions;
using BuildingBlocks.Abstractions.Repository;
using BuildingBlocks.CQRS;
using BuildingBlocks.Enumerations;
using BuildingBlocks.Error;
using BuildingBlocks.Results;
using Microsoft.EntityFrameworkCore;
using ProductService.Application.DTOs;
using ProductService.Domain.Abstractions.Repositories;
using ProductService.Domain.Entities;
using ProductService.Domain.Errors;

namespace ProductService.Application.Commands.Products.Update
{
    public class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand>
    {
        private readonly IFileService _fileService;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProductCommandHandler(
            IProductRepository productRepository, IUnitOfWork unitOfWork, IFileService fileService)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
            _fileService = fileService;
        }

        public async Task<Result> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            Product? product = await _productRepository.AsQueryable().Include(p => p.ProductImages)
                .Include(p => p.ProductSpecs).FirstOrDefaultAsync(p => p.Id == request.ProductId, cancellationToken);

            if (product is null)
                return Result.Failure(ProductError.ProductNotFound);

            Result updateResult = product.UpdateProduct(request.ProductName, request.CategoryId, request.BrandId,
                request.Price, request.Discount, request.Stock, request.Description, request.Specs,
                request.SoldQuantity, request.IsActive, request.IsFeatured);

            if (updateResult.IsFailure)
                return updateResult;

            if (request.ImageIdsToRemove is { Count: > 0 })
            {
                List<string> imageUrlsToDelete = product.ProductImages
                    .Where(img => request.ImageIdsToRemove.Contains(img.Id)).Select(img => img.ImageUrl).ToList();

                foreach (string imageUrl in imageUrlsToDelete)
                {
                    bool deleted = await _fileService.DeleteFile(imageUrl);
                    if (!deleted)
                        return Result.Failure(Error.Problem("FailToDelete", "Cannot delete images."));
                }

                Result removeResult = product.DeleteProductImages(request.ImageIdsToRemove);
                if (removeResult.IsFailure)
                    return removeResult;
            }

            if (request.NewImages is { Count: > 0 } &&
                !request.NewImages.Any(x => string.IsNullOrWhiteSpace(x.ImageContent)))
            {
                List<ProductImage> productImages = new();

                foreach (ProductImageDto imageDto in request.NewImages)
                {
                    string imageUrl = await _fileService.UploadFile(imageDto.ImageContent, AssetType.PRODUCT_IMAGE);

                    Result<ProductImage> imageResult = ProductImage.Create(
                        product.Id, imageUrl, imageDto.IsMain, imageDto.SortOrder);

                    if (imageResult.IsFailure)
                        return Result.Failure(imageResult.Error);

                    productImages.Add(imageResult.Value);
                }

                Result imageAddResult = product.CreateProductImages(productImages);
                if (imageAddResult.IsFailure)
                    return imageAddResult;
            }

            if (request.ProductSpecs is { Count: > 0 })
            {
                List<ProductSpec> newSpecs = new();

                foreach (ProductSpecDto specDto in request.ProductSpecs)
                {
                    Result<ProductSpec> specResult =
                        ProductSpec.Create(product.Id, specDto.SpecName, specDto.SpecValue);
                    if (specResult.IsFailure)
                        return Result.Failure(specResult.Error);

                    newSpecs.Add(specResult.Value);
                }

                Result replaceSpecResult = product.ReplaceProductSpecs(newSpecs);
                if (replaceSpecResult.IsFailure)
                    return replaceSpecResult;
            }

            await _productRepository.UpdateAsync(product, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}