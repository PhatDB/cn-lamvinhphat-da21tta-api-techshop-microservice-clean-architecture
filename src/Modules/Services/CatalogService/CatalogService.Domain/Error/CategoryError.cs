namespace CatalogService.Domain.Error
{
    public static class CategoryError
    {
        public static readonly BuildingBlocks.Error.Error ProductNotFound = BuildingBlocks.Error.Error.NotFound("Product.NotFound", "Product not found");
    }
}