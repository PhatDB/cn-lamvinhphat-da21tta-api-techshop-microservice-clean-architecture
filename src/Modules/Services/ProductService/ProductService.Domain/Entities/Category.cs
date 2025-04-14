using BuildingBlocks.Abstractions.Aggregates;
using BuildingBlocks.Abstractions.Entities;
using BuildingBlocks.Results;
using ProductService.Domain.Errors;

namespace ProductService.Domain.Entities
{
    public class Category : Entity, IAggregateRoot
    {
        private Category(string categoryName, string? description = null, string? imageUrl = null)
        {
            CategoryName = categoryName;
            Description = description;
            ImageUrl = imageUrl;
            IsActive = true;
        }

        public string CategoryName { get; private set; }
        public string? Description { get; private set; }
        public string? ImageUrl { get; private set; }
        public bool IsActive { get; private set; }

        // Create Category
        public static Result<Category> Create(string categoryName, string? description = null, string? imageUrl = null)
        {
            if (string.IsNullOrWhiteSpace(categoryName))
                return Result.Failure<Category>(CategoryError.CategoryNameInvalid);

            return Result.Success(new Category(categoryName, description, imageUrl));
        }

        // Update Category
        public Result UpdateCategory(string? categoryName, string? description, string? imageUrl, bool? isActive)
        {
            CategoryName = categoryName?.Trim() ?? CategoryName;
            Description = description?.Trim() ?? Description;
            ImageUrl = imageUrl?.Trim() ?? ImageUrl;
            IsActive = isActive ?? IsActive;

            return Result.Success();
        }

        // Soft Delete Category
        public Result Delete()
        {
            if (!IsActive)
                return Result.Failure(CategoryError.CategoryAlreadyDeleted);

            IsActive = false;
            return Result.Success();
        }
    }
}