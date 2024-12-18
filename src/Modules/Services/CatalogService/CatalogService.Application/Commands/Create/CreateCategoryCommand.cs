using BuildingBlocks.CQRS;
using CatalogService.Application.DTOs;

namespace CatalogService.Application.Commands.Create
{
    public record CreateCategoryCommand : ICommand<int> 
    {
        public CreateCategoryCommand(string categoryName, string? description, int? parentCategoryId, List<CategoryItemDTO>? categoryItems)
        {
            _categoryItems = categoryItems;
            CategoryName = categoryName;
            Description = description;
            ParentCategoryId = parentCategoryId;
        }

        private readonly List<CategoryItemDTO> _categoryItems;
        public string CategoryName { get; private set; }
        public string? Description { get; private set; }
        public int? ParentCategoryId { get; private set; }
        public IEnumerable<CategoryItemDTO> CategoryItems => _categoryItems;
    }
}