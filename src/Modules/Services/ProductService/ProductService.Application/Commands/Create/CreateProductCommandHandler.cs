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

namespace ProductService.Application.Commands.Create
{
    public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, int>
    {
        private readonly IFileService _fileService;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork, IFileService fileService)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
            _fileService = fileService;
        }

        public async Task<Result<int>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            Product product = Product.Create(request.ProductName, request.Price, request.Description, request.DiscountPrice, request.SKU, request.Brand, request.Model,
                request.StockStatus);

            foreach (ProductImageDTO imageDto in request.ProductImages)
            {
                if (!IsBase64String(imageDto.ImageContent)) return Result.Failure<int>(Error.Validation("Base64.Validation", "Invalid image content"));

                string imageUrl = await _fileService.UploadFile(imageDto.ImageContent, AssetType.PRODUCT_IMAGE);

                Result<ProductImage> imageResult = ProductImage.Create(product.Id, imageUrl, imageDto.AltText);
                if (imageResult.IsFailure) return Result.Failure<int>(imageResult.Error);

                product.AddProductImage(imageResult.Value);
            }

            await _productRepository.AddAsync(product, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(product.Id);
        }

        private bool IsBase64String(string base64String)
        {
            if (string.IsNullOrWhiteSpace(base64String))
                return false;

            base64String = base64String.Trim();

            if (base64String.Length % 4 != 0)
                return false;

            return Regex.IsMatch(base64String, @"^[a-zA-Z0-9+/]*={0,2}$", RegexOptions.None);
        }
    }
}