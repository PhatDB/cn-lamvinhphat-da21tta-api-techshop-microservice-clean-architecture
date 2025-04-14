using BuildingBlocks.Abstractions.Aggregates;
using BuildingBlocks.Abstractions.Entities;
using BuildingBlocks.Error;
using BuildingBlocks.Results;

namespace ProductService.Domain.Entities
{
    public class Brand : Entity, IAggregateRoot
    {
        private readonly List<CategoryBrand> _categoryBrands = new();

        private Brand(string brandName, string? description = null)
        {
            BrandName = brandName;
            Description = description;
            IsActive = true;
            _categoryBrands = new List<CategoryBrand>();
        }

        public string BrandName { get; private set; }
        public string? Description { get; private set; }
        public bool IsActive { get; private set; }
        public IReadOnlyCollection<CategoryBrand> CategoryBrands => _categoryBrands.AsReadOnly();

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

        // Create Category Brand
        public Result CreateCategoryBrands(List<int> categoryIds)
        {
            foreach (int categoryId in categoryIds.Distinct())
            {
                bool exists = _categoryBrands.Any(cb => cb.CategoryId == categoryId);

                if (!exists) _categoryBrands.Add(new CategoryBrand(categoryId, Id));
            }

            return Result.Success();
        }

        // Remove CategoryBrands
        public Result RemoveCategoryBrands(IEnumerable<int> categoryIds)
        {
            List<CategoryBrand> itemsToRemove = _categoryBrands
                .Where(cb => categoryIds.Contains(cb.CategoryId)).ToList();

            if (!itemsToRemove.Any())
                return Result.Failure(Error.NotFound("CategoryBrand.NotFound", "Category Brand is not found"));

            foreach (CategoryBrand item in itemsToRemove) _categoryBrands.Remove(item);

            return Result.Success();
        }
    }
}