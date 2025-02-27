using System.Text.RegularExpressions;
using BuildingBlocks.Abstractions.Extensions;
using BuildingBlocks.Abstractions.Repository;
using BuildingBlocks.CQRS;
using BuildingBlocks.Enumerations;
using BuildingBlocks.Error;
using BuildingBlocks.Results;
using ProductService.Application.DTOs;
using ProductService.Domain.Abstractions.Repositories;
using ProductService.Domain.Entities;

namespace ProductService.Application.Commands.Products.AddImages
{
    public class AddImageCommandHandler : ICommandHandler<AddImageCommand>
    {
        private readonly IFileService _fileService;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AddImageCommandHandler(IProductRepository productRepository, IFileService fileService, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _fileService = fileService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(AddImageCommand request, CancellationToken cancellationToken)
        {
            var productResult = await _productRepository.GetProductDetailAsync(request.ProductId, cancellationToken);
            if (productResult.IsFailure)
                return Result.Failure(Error.NotFound("Product.NotFound", $"Product with ID {request.ProductId} does not exist."));

            var product = productResult.Value;
            
            foreach (ProductImageDTO imageDto in request.ProductImages)
            {
                if (!IsBase64String(imageDto.ImageContent))
                    return Result.Failure(Error.Validation("Base64.Validation", "Invalid image content"));
                
                string imageUrl = await _fileService.UploadFile(imageDto.ImageContent, AssetType.PRODUCT_IMAGE);
                
                var imageResult = ProductImage.Create(product.Id, imageUrl, imageDto.Position, imageDto.Title);
                if (imageResult.IsFailure) return Result.Failure(imageResult.Error);
                
                var addImageResult = product.AddProductImage(imageResult.Value.ImageUrl, imageResult.Value.Position, imageResult.Value.Title);

                if (addImageResult.IsFailure)
                    return Result.Failure<int>(addImageResult.Error);
            }
            
            await _productRepository.UpdateAsync(product, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        private bool IsBase64String(string base64String)
        {
            if (string.IsNullOrWhiteSpace(base64String)) return false;
            base64String = base64String.Trim();
            return base64String.Length % 4 == 0 && Regex.IsMatch(base64String, @"^[a-zA-Z0-9+/]*={0,2}$", RegexOptions.None);
        }
    }
}
