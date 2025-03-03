using BuildingBlocks.Abstractions.Aggregates;
using BuildingBlocks.Abstractions.Entities;
using BuildingBlocks.Error;
using BuildingBlocks.Results;

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
                return Result.Failure<Category>(Error.Validation("Category.EmptyName",
                    "Category name cannot be empty."));

            return Result.Success(new Category(name, description));
        }

        public void Deactivate()
        {
            IsActive = false;
        }
    }
}