using BuildingBlocks.Abstractions.Aggregates;
using BuildingBlocks.Abstractions.Entities;
using ProductService.Domain.ValueObjects;
using BuildingBlocks.Results;
using BuildingBlocks.Error;

namespace ProductService.Domain.Entities
{
    public class Product : Entity, IAggregateRoot
    {
        private readonly List<ProductImage> _productImages;
        private readonly List<ProductColor> _productColors;

        private Product(string name, SKU sku, decimal price, int categoryId, string? description = null, decimal? discountPrice = null)
        {
            Name = name;
            Sku = sku;
            Price = price;
            CategoryId = categoryId;
            Description = description;
            DiscountPrice = discountPrice;
            SoldQuantity = 0;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            IsActive = true;

            _productImages = new List<ProductImage>();
            _productColors = new List<ProductColor>();
        }

        private Product() { }

        public string Name { get; private set; }
        public SKU Sku { get; private set; }
        public string? Description { get; private set; }
        public decimal Price { get; private set; }
        public decimal? DiscountPrice { get; private set; }
        public int SoldQuantity { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public bool IsActive { get; private set; }
        public int CategoryId { get; private set; }

        public IReadOnlyCollection<ProductImage> ProductImages => _productImages.AsReadOnly();
        public IReadOnlyCollection<ProductColor> ProductColors => _productColors.AsReadOnly();

        public static Result<Product> Create(string name, string sku, decimal price, int categoryId, string? description = null, decimal? discountPrice = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result.Failure<Product>(Error.Validation("Product.EmptyName", "Product name cannot be empty."));

            if (price <= 0)
                return Result.Failure<Product>(Error.Validation("Product.InvalidPrice", "Price must be greater than zero."));

            var skuResult = SKU.Create(sku);
            if (skuResult.IsFailure)
                return Result.Failure<Product>(skuResult.Error);

            return Result.Success(new Product(name, skuResult.Value, price, categoryId, description, discountPrice));
        }

        public Result UpdateStock(int quantity)
        {
            if (quantity < 0 && SoldQuantity + quantity < 0)
                return Result.Failure(Error.Validation("Product.InsufficientStock", "Not enough stock available."));

            SoldQuantity += quantity;
            UpdatedAt = DateTime.UtcNow;
            return Result.Success();
        }

        public Result AddProductImage(string imageUrl, int position, string? title = null)
        {
            if (string.IsNullOrWhiteSpace(imageUrl))
                return Result.Failure(Error.Validation("ProductImage.InvalidUrl", "Image URL cannot be empty."));

            _productImages.Add(new ProductImage(Id, imageUrl, position, title));
            UpdatedAt = DateTime.UtcNow;
            return Result.Success();
        }
        
        public Result AddColor(Color color)
        {
            if (color == null)
                return Result.Failure(Error.Validation("ProductColor.NullColor", "Color cannot be null."));
            
            if (_productColors.Any(pc => pc.ColorId == color.Id))
                return Result.Failure(Error.Conflict("ProductColor.Duplicate", "Color already exists for this product."));

            _productColors.Add(new ProductColor(Id, color.Id));
            UpdatedAt = DateTime.UtcNow;
            return Result.Success();
        }
        
        public Result Deactivate()
        {
            if (!IsActive)
                return Result.Failure(Error.Validation("Product.AlreadyInactive", "Product is already inactive."));

            IsActive = false;
            UpdatedAt = DateTime.UtcNow;
            return Result.Success();
        }
    }
}
