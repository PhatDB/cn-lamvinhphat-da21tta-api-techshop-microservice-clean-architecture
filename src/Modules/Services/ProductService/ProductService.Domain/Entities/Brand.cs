using BuildingBlocks.Abstractions.Aggregates;
using BuildingBlocks.Abstractions.Entities;
using BuildingBlocks.Results;

namespace ProductService.Domain.Entities
{
    public class Brand : Entity, IAggregateRoot
    {
        private Brand(string brandName, string? description = null)
        {
            BrandName = brandName;
            Description = description;
            IsActive = true;
        }

        public string BrandName { get; private set; }
        public string? Description { get; private set; }
        public bool IsActive { get; private set; }

        // Create Brand
        public static Result<Brand> Create(string brandName, string? description)
        {
            return new Brand(brandName, description);
        }

        // Update Brand
        public Result UpdateBrand(string? brandName, string? description, bool? isActive)
        {
            BrandName = brandName ?? BrandName;
            Description = description ?? Description;
            IsActive = isActive ?? IsActive;

            return Result.Success();
        }

        // Soft Delete Brand
        public Result DeleteBrand()
        {
            IsActive = false;
            return Result.Success();
        }
    }
}