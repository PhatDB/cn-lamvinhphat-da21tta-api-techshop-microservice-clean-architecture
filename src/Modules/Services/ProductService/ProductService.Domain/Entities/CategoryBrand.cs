using BuildingBlocks.Abstractions.Entities;
using BuildingBlocks.Results;

namespace ProductService.Domain.Entities
{
    public class CategoryBrand : Entity
    {
        public CategoryBrand(int categoryId, int brandId)
        {
            CategoryId = categoryId;
            BrandId = brandId;
        }

        public int CategoryId { get; set; }
        public int BrandId { get; set; }

        // Create Category Brand
        public static Result<CategoryBrand> Create(int categoryId, int brandId)
        {
            return new CategoryBrand(categoryId, brandId);
        }
    }
}