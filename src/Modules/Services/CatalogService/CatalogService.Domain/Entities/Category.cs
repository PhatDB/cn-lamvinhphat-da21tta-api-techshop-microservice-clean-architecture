using CatalogService.Domain.Abstractions.Aggregates;
using CatalogService.Domain.ValueObjects;

namespace CatalogService.Domain.Entities
{
    public class Category : AggregateRoot<CategoryId>
    {
        private Category()
        {
        }

        public Name CategoryName { get; private set; }
        public string? Description { get; private set; }
        public CategoryId? ParentCategoryId { get; private set; }
        public DateTime CreatedDate { get; private set; }
        public DateTime UpdatedDate { get; private set; }
        public bool IsActive { get; private set; }


        public static Category Create(CategoryId id, Name categoryName, string? description, CategoryId? parentCategoryId, DateTime createdDate, bool isActive)
        {
            return new Category
            {
                Id = id,
                CategoryName = categoryName,
                Description = description,
                ParentCategoryId = parentCategoryId,
                CreatedDate = createdDate,
                IsActive = isActive
            };
        }

        public void Update(Name categoryName, string? description, DateTime updatedDate, bool isActive)
        {
            CategoryName = categoryName;
            Description = description;
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
    }
}