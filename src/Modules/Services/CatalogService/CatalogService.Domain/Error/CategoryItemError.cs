namespace CatalogService.Domain.Error
{
    public static class CategoryItemError
    {
        public static readonly BuildingBlocks.Error.Error CatelogryIdInvalid = BuildingBlocks.Error.Error.Validation("CategoryId.Validation", "CategoryId is Invalid");

        public static readonly BuildingBlocks.Error.Error ProductIdInvalid = BuildingBlocks.Error.Error.Validation("ProductId.Validation", "ProductId is Invalid");

        public static BuildingBlocks.Error.Error NotFound(int id)
        {
            return BuildingBlocks.Error.Error.Validation("CategoryItem.NotFound", $"CategoryItem with the Id = '{id}' was not found");
        }

        public static BuildingBlocks.Error.Error ProductAllReadyExist(int id)
        {
            return BuildingBlocks.Error.Error.Validation("CategoryItem.Existed", $"Product with the Id = '{id}' Existed");
        }
    }
}