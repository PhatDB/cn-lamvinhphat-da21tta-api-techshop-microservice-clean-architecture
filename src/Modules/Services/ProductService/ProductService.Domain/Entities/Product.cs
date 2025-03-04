using BuildingBlocks.Abstractions.Aggregates;
using BuildingBlocks.Abstractions.Entities;
using BuildingBlocks.Results;
using ProductService.Domain.Errors;
using ProductService.Domain.ValueObjects;

namespace ProductService.Domain.Entities
{
    public class Product : Entity, IAggregateRoot
    {
        private readonly List<ProductColor> _productColors;
        private readonly List<ProductImage> _productImages;

        private Product(
            string name, SKU sku, decimal price, int categoryId,
            string? description = null, decimal? discountPrice = null)
        {
            Name = name;
            Sku = sku;
            Price = price;
            CategoryId = categoryId;
            Description = description;
            DiscountPrice = discountPrice;
            SoldQuantity = 0;
            CreatedAt = DateTime.UtcNow;
            IsActive = true;

            _productImages = new List<ProductImage>();
            _productColors = new List<ProductColor>();
        }


        private Product()
        {
            _productImages = new List<ProductImage>();
            _productColors = new List<ProductColor>();
        }

        public string Name { get; private set; }
        public SKU Sku { get; private set; }
        public string? Description { get; private set; }
        public decimal Price { get; private set; }
        public decimal? DiscountPrice { get; private set; }
        public int SoldQuantity { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public bool IsActive { get; private set; }
        public int CategoryId { get; private set; }

        public IReadOnlyCollection<ProductImage> ProductImages =>
            _productImages.AsReadOnly();

        public IReadOnlyCollection<ProductColor> ProductColors =>
            _productColors.AsReadOnly();

        public static Result<Product> Create(
            string name, string sku, decimal price, int categoryId,
            string? description = null, decimal? discountPrice = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result.Failure<Product>(ProductError.ProductNameInvalid);

            if (price <= 0)
                return Result.Failure<Product>(ProductError.ProductPriceInvalid);

            Result<SKU> skuResult = SKU.Create(sku);
            if (skuResult.IsFailure)
                return Result.Failure<Product>(skuResult.Error);

            return Result.Success(new Product(name, skuResult.Value, price, categoryId,
                description, discountPrice));
        }

        public Result UpdateStock(int quantity)
        {
            if (quantity < 0 && SoldQuantity + quantity < 0)
                return Result.Failure(ProductError.ProductInsufficientStock);

            SoldQuantity += quantity;
            return Result.Success();
        }

        public Result AddProductImage(string imageUrl, int position, string? title = null)
        {
            if (string.IsNullOrWhiteSpace(imageUrl))
                return Result.Failure(ProductImageError.ProductImageInvalid);

            _productImages.Add(new ProductImage(Id, imageUrl, position, title));

            return Result.Success();
        }

        public Result AddColor(Color color, int stockQuantity)
        {
            if (color == null)
                return Result.Failure(ProductColorError.ProductColorNullColor);

            if (_productColors.Any(pc => pc.ColorId == color.Id))
                return Result.Failure(ProductColorError.ProductColorDuplicate);

            _productColors.Add(new ProductColor(Id, color.Id, stockQuantity));
            return Result.Success();
        }

        public Result AddOrUpdateColor(Color color, int stockQuantity)
        {
            ProductColor? existingColor =
                _productColors.FirstOrDefault(pc => pc.ColorId == color.Id);

            if (existingColor != null)
                existingColor.UpdateStock(stockQuantity);
            else
                _productColors.Add(new ProductColor(Id, color.Id, stockQuantity));

            return Result.Success();
        }

        public Result UpdateColorStock(int colorId, int newStockQuantity)
        {
            ProductColor? productColor =
                _productColors.FirstOrDefault(pc => pc.ColorId == colorId);
            if (productColor == null)
                return Result.Failure(ProductColorError.ProductColorNotFound);

            productColor.UpdateStock(newStockQuantity);
            return Result.Success();
        }


        public Result DeleteProduct()
        {
            if (!IsActive)
                return Result.Failure(ProductError.ProductAlreadyDeleted);

            IsActive = false;
            return Result.Success();
        }

        public Result UpdateProduct(
            string? name, string? sku, decimal? price, int? categoryId, int? soldQuantity,
            bool? isActive, string? description, decimal? discountPrice)
        {
            Name = name?.Trim() ?? Name;
            Description = description?.Trim() ?? Description;
            DiscountPrice = discountPrice ?? DiscountPrice;
            CategoryId = categoryId ?? CategoryId;
            SoldQuantity = soldQuantity ?? SoldQuantity;
            IsActive = isActive ?? IsActive;

            if (!string.IsNullOrWhiteSpace(sku))
            {
                Result<SKU> skuResult = SKU.Create(sku);
                if (skuResult.IsFailure)
                    return Result.Failure(skuResult.Error);
                Sku = skuResult.Value;
            }

            Price = price ?? Price;

            return Result.Success();
        }

        public Result RemoveProductImages(IEnumerable<int> imageIds)
        {
            List<ProductImage> imagesToRemove =
                _productImages.Where(img => imageIds.Contains(img.Id)).ToList();

            if (!imagesToRemove.Any())
                return Result.Failure(ProductImageError.ProductImageNotFound);

            foreach (ProductImage image in imagesToRemove) _productImages.Remove(image);

            return Result.Success();
        }
    }
}