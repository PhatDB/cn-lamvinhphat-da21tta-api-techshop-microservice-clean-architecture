using BuildingBlocks.Abstractions.Aggregates;
using BuildingBlocks.Abstractions.Entities;

namespace ProductService.Domain.Entities
{
    public class Product : Entity, IAggregateRoot
    {
        private readonly List<ProductImage> _productImages;

        private Product(
            string productName, decimal price, string? description = null, decimal? discountPrice = null, string? sku = null, string? brand = null, string? model = null,
            int? stockStatus = null)
        {
            ProductName = productName;
            Description = description;
            Price = price;
            DiscountPrice = discountPrice;
            SKU = sku;
            Brand = brand;
            Model = model;
            StockStatus = stockStatus;
            CreatedDate = DateTime.UtcNow;
            UpdatedDate = DateTime.UtcNow;
            IsActive = true;
            _productImages = new List<ProductImage>();
        }

        private Product()
        {
        }

        public string ProductName { get; private set; }
        public string? Description { get; private set; }
        public decimal Price { get; private set; }
        public decimal? DiscountPrice { get; private set; }
        public string? SKU { get; private set; }
        public string? Brand { get; private set; }
        public string? Model { get; private set; }
        public int? StockStatus { get; private set; }
        public DateTime CreatedDate { get; private set; }
        public DateTime UpdatedDate { get; private set; }
        public bool IsActive { get; private set; }

        public IReadOnlyCollection<ProductImage> ProductImages => _productImages.AsReadOnly();

        public static Product Create(
            string productName, decimal price, string? description = null, decimal? discountPrice = null, string? sku = null, string? brand = null, string? model = null,
            int? stockStatus = null)
        {
            return new Product(productName, price, description, discountPrice, sku, brand, model, stockStatus);
        }

        public void UpdateProduct(
            string? newProductName, decimal? newPrice, string? newDescription = null, decimal? newDiscountPrice = null, string? newSku = null, string? newBrand = null,
            string? newModel = null, int? newStockStatus = null)
        {
            ProductName = newProductName ?? ProductName;
            Description = newDescription ?? Description;
            DiscountPrice = newDiscountPrice ?? DiscountPrice;
            SKU = newSku ?? SKU;
            Brand = newBrand ?? Brand;
            Model = newModel ?? Model;
            StockStatus = newStockStatus ?? StockStatus;
            Price = newPrice ?? Price;
            UpdatedDate = DateTime.UtcNow;
        }


        public void DeleteProduct()
        {
            IsActive = false;
            UpdatedDate = DateTime.UtcNow;
        }

        public void AddProductImage(ProductImage image)
        {
            _productImages.Add(image);
            UpdatedDate = DateTime.UtcNow;
        }

        public void RemoveProductImages(IEnumerable<int> imageIds)
        {
            List<ProductImage> imagesToRemove = ProductImages.Where(img => imageIds.Contains(img.Id)).ToList();
            foreach (ProductImage image in imagesToRemove) _productImages.Remove(image);
            UpdatedDate = DateTime.UtcNow;
        }
    }
}