using BuildingBlocks.Abstractions.Entities;
using BuildingBlocks.Results;
using CatalogService.Domain.Error;

namespace CatalogService.Domain.Entities
{
    public class CategoryItem : Entity
    {
        private CategoryItem(int categoryId, int productId)
        {
            CategoryId = categoryId;
            ProductId = productId;
            CreatedDate = DateTime.UtcNow;
        }

        public int CategoryId { get; private set; }
        public int ProductId { get; private set; }
        public DateTime CreatedDate { get; private set; }

        internal static Result<CategoryItem> Create(int categoryId, int productId)
        {
            if (categoryId <= 0)
                return Result.Failure<CategoryItem>(CategoryItemError.CatelogryIdInvalid);
            if (productId <= 0)
                return Result.Failure<CategoryItem>(CategoryItemError.ProductIdInvalid);
            CategoryItem categoryItem = new(categoryId, productId);
            return categoryItem;
        }
    }
}