using System.Text.RegularExpressions;
using BuildingBlocks.Abstractions.Extensions;
using BuildingBlocks.Abstractions.Repository;
using BuildingBlocks.CQRS;
using BuildingBlocks.Enumerations;
using BuildingBlocks.Results;
using Microsoft.EntityFrameworkCore;
using ProductService.Application.DTOs;
using ProductService.Domain.Abstractions.Repositories;
using ProductService.Domain.Entities;
using ProductService.Domain.Errors;

namespace ProductService.Application.Commands.Products.Create
{
    public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, int>
    {
        private readonly IFileService _fileService;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;


        public CreateProductCommandHandler(
            IProductRepository productRepository, IUnitOfWork unitOfWork,
            IFileService fileService)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
            _fileService = fileService;
        }

        public async Task<Result<int>> Handle(
            CreateProductCommand request, CancellationToken cancellationToken)
        {
            Result<Product> productResult = Product.Create(request.Name, request.SKU,
                request.Price, request.CategoryId, request.Description,
                request.DiscountPrice);
            if (productResult.IsFailure)
                return Result.Failure<int>(productResult.Error);

            Product product = productResult.Value;

            bool skuExists = (await _productRepository.AsQueryable()
                    .AsNoTracking().Select(p => new { p.Id, SkuValue = p.Sku.Value })
                    .ToListAsync(cancellationToken))
                .Any(p => p.SkuValue == request.SKU.ToUpper());

            if (skuExists)
                return Result.Failure<int>(ProductError.ProductSkuDuplicate);

            product.CreateInventory(request.Inventory.StockQuantity);

            foreach (ProductImageDTO imageDto in request.Images)
            {
                if (!IsBase64String(imageDto.ImageContent))
                    return Result.Failure<int>(
                        ProductImageError.ProductImageBase64Invalid);

                string imageUrl = await _fileService.UploadFile(imageDto.ImageContent,
                    AssetType.PRODUCT_IMAGE);

                Result<ProductImage> imageResult = ProductImage.Create(product.Id,
                    imageUrl, imageDto.Position, imageDto.Title);
                if (imageResult.IsFailure) return Result.Failure<int>(imageResult.Error);

                Result addImageResult = product.AddProductImage(
                    imageResult.Value.ImageUrl, imageResult.Value.Position,
                    imageResult.Value.Title);

                if (addImageResult.IsFailure)
                    return Result.Failure<int>(addImageResult.Error);
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

            return Regex.IsMatch(base64String, @"^[a-zA-Z0-9+/]*={0,2}$",
                RegexOptions.None);
        }
    }
}