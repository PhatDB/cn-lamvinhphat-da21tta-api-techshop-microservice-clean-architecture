using BuildingBlocks.Abstractions.Aggregates;
using BuildingBlocks.Abstractions.Entities;
using BuildingBlocks.Results;
using CatalogService.Domain.Error;

namespace CatalogService.Domain.Entities
{
    public class Category : Entity, IAggregateRoot
    {
        private readonly List<CategoryItem> _categoryItems;

        private Category(string categoryName, string? description, int? parentCategoryId)
        {
            CategoryName = categoryName;
            Description = description;
            ParentCategoryId = parentCategoryId;
            CreatedDate = DateTime.UtcNow;
            UpdatedDate = DateTime.UtcNow;
            IsActive = true;
            _categoryItems = new List<CategoryItem>();
        }

        private Category()
        {
        }

        public IReadOnlyCollection<CategoryItem> CategoryItems => _categoryItems.AsReadOnly();

        public string CategoryName { get; private set; }
        public string? Description { get; private set; }
        public int? ParentCategoryId { get; private set; }
        public DateTime CreatedDate { get; private set; }
        public DateTime UpdatedDate { get; private set; }
        public bool IsActive { get; private set; }

        public static Category Create(string categoryName, string? description = null, int? parentCategoryId = null)
        {
            return new Category(categoryName, description, parentCategoryId);
        }

        public void UpdateCategory(string newCategoryName, string? newDescription = null, int? newParentCategoryId = null)
        {
            CategoryName = newCategoryName;
            Description = newDescription;
            ParentCategoryId = newParentCategoryId;
            UpdatedDate = DateTime.UtcNow;
        }

        public void Delete()
        {
            IsActive = false;
            UpdatedDate = DateTime.UtcNow;
        }

        public void ToggleActivation()
        {
            IsActive = !IsActive;
            UpdatedDate = DateTime.UtcNow;
        }

        public Result AddCategoryItem(int productId)
        {
            Result<CategoryItem> result = CategoryItem.Create(productId, Id);
            if (result.IsFailure)
                return result;

            CategoryItem? existItem = _categoryItems.FirstOrDefault(x => x.ProductId == productId && x.CategoryId == Id);
            if (existItem != null)
                return Result.Failure(CategoryItemError.ProductAllReadyExist(productId));

            _categoryItems.Add(result.Value);
            UpdatedDate = DateTime.UtcNow;
            return Result.Success();
        }

        public Result RemoveCategoryItem(int categoryItemId)
        {
            CategoryItem? categoryItem = _categoryItems.Find(x => x.Id == categoryItemId);

            if (categoryItem is null) return Result.Failure(CategoryItemError.NotFound(categoryItemId));

            _categoryItems.Remove(categoryItem);
            UpdatedDate = DateTime.UtcNow;
            return Result.Success();
        }
    }
}