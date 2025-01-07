using BuildingBlocks.Abstractions.Entities;
using BuildingBlocks.Results;
using CatalogService.Domain.Error;

namespace CatalogService.Domain.Entities
{
    public class CategoryItem : Entity
    {
        public int CategoryId { get; private set; }
        public int ProductId { get; private set; }
        public DateTime CreatedDate { get; private set; }

        private CategoryItem(int categoryId, int productId)
        {
            CategoryId = categoryId;
            ProductId = productId;
            CreatedDate = DateTime.UtcNow;
        }

        internal static Result<CategoryItem> Create(int categoryId, int productId)
        {
            if (productId <= 0)
                return Result.Failure<CategoryItem>(CategoryItemError.ProductIdInvalid);
            CategoryItem categoryItem = new(categoryId, productId);
            return categoryItem;
        }
    }
}