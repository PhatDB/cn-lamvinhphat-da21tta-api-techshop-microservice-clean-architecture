using BuildingBlocks.Extensions;
using CatalogService.Domain.Abstractions.Aggregates;
using CatalogService.Domain.Enumerations;
using CatalogService.Domain.ValueObjects;

namespace CatalogService.Domain.Entities
{
    public class Product : AggregateRoot<ProductId>
    {
        private readonly List<ProductImages> _productImages = new();

        private Product()
        {
        }

        public IReadOnlyList<ProductImages> ProductImages => _productImages.AsReadOnly();

        public Name ProductName { get; private set; }
        public StockStatus StockStatus { get; private set; }
        public string SKU { get; private set; }
        public PriceValueObject PriceInfo { get; private set; }
        public ProductDetails Details { get; private set; }
        public DateTime CreatedDate { get; private set; }
        public DateTime UpdatedDate { get; private set; }
        public bool IsActive { get; private set; }

        public decimal FinalPrice => PriceInfo.DiscountPrice.HasValue ? PriceInfo.Price - PriceInfo.DiscountPrice.Value : PriceInfo.Price;

        public static Product Create(
            ProductId id, Name productName, StockStatus stockStatus, string sku, PriceValueObject priceInfo, ProductDetails details, DateTime createdDate, bool isActive)
        {
            return new Product
            {
                Id = id,
                ProductName = productName,
                StockStatus = stockStatus,
                SKU = sku,
                PriceInfo = priceInfo,
                Details = details,
                CreatedDate = createdDate,
                IsActive = isActive
            };
        }

        public void Update(Name productName, StockStatus stockStatus, string sku, PriceValueObject priceInfo, ProductDetails details, DateTime updatedDate, bool isActive)
        {
            ProductName = productName;
            StockStatus = stockStatus;
            SKU = sku;
            PriceInfo = priceInfo;
            Details = details;
            UpdatedDate = updatedDate;
            IsActive = isActive;
        }

        public void Deactivate(DateTime updatedDate)
        {
            IsActive = false;
            UpdatedDate = updatedDate;
        }

        public void Activate(DateTime updatedDate)
        {
            IsActive = true;
            UpdatedDate = updatedDate;
        }

        public void AddProductImage(ProductImages productImages)
        {
            Ensure.NotNull(productImages);
            _productImages.Add(productImages);
        }
    }
}