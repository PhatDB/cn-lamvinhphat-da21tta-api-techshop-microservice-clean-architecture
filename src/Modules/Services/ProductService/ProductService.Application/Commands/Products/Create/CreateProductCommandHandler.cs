using BuildingBlocks.Abstractions.Extensions;
using BuildingBlocks.Abstractions.Repository;
using BuildingBlocks.CQRS;
using BuildingBlocks.Enumerations;
using BuildingBlocks.Results;
using ProductService.Application.DTOs;
using ProductService.Domain.Abstractions.Repositories;
using ProductService.Domain.Entities;

namespace ProductService.Application.Commands.Products.Create
{
    public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, int>
    {
        private readonly IFileService _fileService;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateProductCommandHandler(
            IFileService fileService, IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            _fileService = fileService;
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<int>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            Result<Product> productResult = Product.Create(request.ProductName, request.CategoryId, request.BrandId,
                request.Price, request.Discount, request.Stock, request.Description, request.Specs);

            if (productResult.IsFailure)
                return Result.Failure<int>(productResult.Error);

            Product product = productResult.Value;

            List<ProductImage> productImages = new();

            foreach (ProductImageDto imageDto in request.Images)
            {
                string imageUrl = await _fileService.UploadFile(imageDto.ImageContent, AssetType.PRODUCT_IMAGE);

                Result<ProductImage> imageResult = ProductImage.Create(product.Id,
                    imageUrl, imageDto.IsMain, imageDto.SortOrder);
                if (imageResult.IsFailure) return Result.Failure<int>(imageResult.Error);

                productImages.Add(imageResult.Value);
            }

            Result addImageResult = product.CreateProductImages(productImages);

            if (addImageResult.IsFailure)
                return Result.Failure<int>(addImageResult.Error);

            await _productRepository.AddAsync(product, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(product.Id);
        }
    }
}