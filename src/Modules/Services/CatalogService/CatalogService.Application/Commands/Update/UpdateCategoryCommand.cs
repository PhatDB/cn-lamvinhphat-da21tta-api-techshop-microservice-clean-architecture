using BuildingBlocks.CQRS;
using BuildingBlocks.Results;

namespace CatalogService.Application.Commands.Update
{
    public class UpdateCategoryCommand : ICommand
    {
        public int CategoryId { get; }
        public string CategoryName { get; }
        public string? Description { get; }
        public int? ParentCategoryId { get; }
        
        public UpdateCategoryCommand(int categoryId, string? categoryName, string? description, int? parentCategoryId)
        {
            CategoryId = categoryId;
            CategoryName = categoryName;
            Description = description;
            ParentCategoryId = parentCategoryId;
        }
    }
}