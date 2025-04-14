using BuildingBlocks.Abstractions.Aggregates;
using BuildingBlocks.Abstractions.Entities;
using BuildingBlocks.Results;
using ProductService.Domain.Errors;
using ProductService.Domain.Events;

namespace ProductService.Domain.Entities
{
    public class Product : Entity, IAggregateRoot
    {
        private readonly List<ProductImage> _productImages;

        private Product(
            string productName, int categoryId, int brandId, decimal price, decimal? discount, int stock,
            string? description = null, string? specs = null)
        {
            ProductName = productName;
            CategoryId = categoryId;
            BrandId = brandId;
            Price = price;
            Discount = discount ?? 0;
            Stock = stock;
            Description = description ?? string.Empty;
            Specs = specs ?? string.Empty;
            IsActive = true;
            IsFeatured = false;
            SoldQuantity = 0;
            CreatedAt = DateTime.UtcNow;
            _productImages = new List<ProductImage>();
        }

        private Product()
        {
        }

        public string ProductName { get; private set; }
        public int CategoryId { get; private set; }
        public int BrandId { get; private set; }
        public decimal Price { get; private set; }
        public decimal? Discount { get; private set; }
        public int Stock { get; private set; }
        public string? Description { get; private set; }
        public string? Specs { get; private set; }
        public bool IsActive { get; private set; }
        public bool IsFeatured { get; private set; }
        public int SoldQuantity { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public IReadOnlyCollection<ProductImage> ProductImages => _productImages.AsReadOnly();

        // Create Product
        public static Result<Product> Create(
            string productName, int categoryId, int brandId, decimal price, decimal? discount, int stock,
            string? description, string? specs)
        {
            if (string.IsNullOrWhiteSpace(productName))
                return Result.Failure<Product>(ProductError.ProductNameInvalid);

            if (price <= 0)
                return Result.Failure<Product>(ProductError.ProductPriceInvalid);

            Product product = new(productName, categoryId, brandId, price, discount, stock, description, specs);

            product.AddDomainEvent(new ProductCreatedDomainEvent(product.Id, product.ProductName, product.Price));

            return Result.Success(product);
        }

        // Update Stock
        public Result UpdateStock(int quantity)
        {
            Stock += quantity;
            return Result.Success();
        }

        // Update Product
        public Result UpdateProduct(
            string? productName, int? categoryId, int? brandId, decimal? price, decimal? discount, int? stock,
            string? description, string? specs, int? soldQuantity, bool? isActive, bool? isFeatured)
        {
            ProductName = productName?.Trim() ?? ProductName;
            CategoryId = categoryId ?? CategoryId;
            BrandId = brandId ?? BrandId;
            Price = price ?? Price;
            Discount = discount ?? Discount;
            Stock = stock ?? Stock;
            Description = description?.Trim() ?? Description;
            Specs = specs?.Trim() ?? Specs;
            SoldQuantity = soldQuantity ?? SoldQuantity;
            IsActive = isActive ?? IsActive;
            IsFeatured = isFeatured ?? IsFeatured;

            return Result.Success();
        }

        // Soft Delete Product
        public Result DeleteProduct()
        {
            if (!IsActive)
                return Result.Failure(ProductError.ProductAlreadyDeleted);

            IsActive = false;
            return Result.Success();
        }

        // Create Product Images
        public Result CreateProductImages(List<ProductImage> productImages)
        {
            if (!productImages.Any())
                return Result.Failure(ProductImageError.ProductImageInvalid);

            foreach (ProductImage image in productImages)
            {
                if (string.IsNullOrWhiteSpace(image.ImageUrl))
                    return Result.Failure(ProductImageError.ProductImageInvalid);

                _productImages.Add(image);
            }

            return Result.Success();
        }

        // Delete Product Image
        public Result DeleteProductImages(IEnumerable<int> imageIds)
        {
            List<ProductImage> imagesToRemove = _productImages.Where(img => imageIds.Contains(img.Id)).ToList();

            if (!imagesToRemove.Any())
                return Result.Failure(ProductImageError.ProductImageNotFound);

            foreach (ProductImage image in imagesToRemove) _productImages.Remove(image);

            return Result.Success();
        }
    }
}