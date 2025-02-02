﻿using System.Text.RegularExpressions;
using BuildingBlocks.Abstractions.Extensions;
using BuildingBlocks.Abstractions.Repository;
using BuildingBlocks.CQRS;
using BuildingBlocks.Enumerations;
using BuildingBlocks.Error;
using BuildingBlocks.Results;
using ProductService.Application.DTOs;
using ProductService.Domain.Abstractions.Repositories;
using ProductService.Domain.Entities;

namespace ProductService.Application.Commands.AddImages
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
            Product product = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken);
            if (product == null)
            {
                Error error = new("ProductNotFound", $"Product with ID {request.ProductId} does not exist.", ErrorType.NotFound);
                return Result.Failure(error);
            }

            foreach (ProductImageDTO imageDto in request.ProductImages)
            {
                if (!IsBase64String(imageDto.ImageContent)) return Result.Failure(Error.Validation("Base64.Validation", "Invalid image content"));

                string imageUrl = await _fileService.UploadFile(imageDto.ImageContent, AssetType.PRODUCT_IMAGE);

                Result<ProductImage> imageResult = ProductImage.Create(product.Id, imageUrl, imageDto.AltText);
                if (imageResult.IsFailure) return Result.Failure(imageResult.Error);

                product.AddProductImage(imageResult.Value);
            }

            _productRepository.Update(product, cancellationToken);
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