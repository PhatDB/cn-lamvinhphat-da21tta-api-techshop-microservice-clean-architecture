using BuildingBlocks.Abstractions.Aggregates;
using BuildingBlocks.Abstractions.Entities;
using BuildingBlocks.Results;
using ProductService.Domain.Errors;

namespace ProductService.Domain.Entities
{
    public class Category : Entity, IAggregateRoot
    {
        private Category(string name, string? description)
        {
            Name = name;
            Description = description;
            CreatedAt = DateTime.UtcNow;
            IsActive = true;
        }

        public string Name { get; private set; }
        public string? Description { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public bool IsActive { get; private set; }

        public static Result<Category> Create(string name, string? description = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result.Failure<Category>(CategoryError.CategoryNameInvalid);

            return Result.Success(new Category(name, description));
        }

        public Result Update(string? name, string? description, bool? isActive)
        {
            Name = name?.Trim() ?? Name;
            Description = description?.Trim() ?? Description;
            IsActive = isActive ?? IsActive;

            return Result.Success();
        }

        public Result Delete()
        {
            if (!IsActive)
                return Result.Failure(CategoryError.CategoryAlreadyDeleted);

            IsActive = false;
            return Result.Success();
        }
    }
}