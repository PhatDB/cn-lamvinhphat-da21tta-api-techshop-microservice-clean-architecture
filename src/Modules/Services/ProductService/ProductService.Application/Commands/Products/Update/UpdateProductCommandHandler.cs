using BuildingBlocks.Abstractions.Repository;
using BuildingBlocks.CQRS;
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
        private readonly IColorRepository _colorRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProductCommandHandler(
            IProductRepository productRepository, IColorRepository colorRepository,
            IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _colorRepository = colorRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(
            UpdateProductCommand request, CancellationToken cancellationToken)
        {
            Product? productResult = await _productRepository.AsQueryable()
                .Include(p => p.ProductColors).Where(p => p.Id == request.ProductId)
                .FirstOrDefaultAsync(cancellationToken);

            if (productResult == null)
                return Result.Failure(ProductError.ProductNotFound);

            Product product = productResult;

            if (!string.IsNullOrWhiteSpace(request.Sku))
            {
                bool isSkuDuplicate = (await _productRepository.AsQueryable()
                    .Select(p => new { p.Id, SkuValue = p.Sku.Value })
                    .ToListAsync(cancellationToken)).Any(p =>
                    p.SkuValue == request.Sku.ToUpper() && p.Id != request.ProductId);

                if (isSkuDuplicate)
                    return Result.Failure(ProductError.ProductSkuDuplicate);
            }

            if (request.Colors != null && request.Colors.Any())
                foreach (ColorDTO colorDto in request.Colors)
                {
                    Result<Color> colorResult =
                        await _colorRepository.GetColorByNameAsync(colorDto.ColorName,
                            cancellationToken);

                    if (colorResult.IsFailure)
                    {
                        colorResult = Color.Create(colorDto.ColorName);
                        if (colorResult.IsFailure)
                            return Result.Failure(colorResult.Error);
                        await _colorRepository.AddAsync(colorResult.Value,
                            cancellationToken);
                        await _unitOfWork.SaveChangesAsync(cancellationToken);
                    }

                    product.AddOrUpdateColor(colorResult.Value,
                        colorDto.StockQuantity.Value);
                }

            product.UpdateProduct(request.Name, request.Sku, request.Price,
                request.CategoryId, request.SoldQuantity, request.IsActive,
                request.Description, request.DiscountPrice);

            await _productRepository.UpdateAsync(product, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}