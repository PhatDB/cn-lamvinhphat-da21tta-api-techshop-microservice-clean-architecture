using BuildingBlocks.CQRS;
using CatalogService.Application.DTOs;

namespace CatalogService.Application.Commands.Create
{
    public record CreateCategoryCommand : ICommand<int>
    {
        private readonly List<CategoryItemDTO> _categoryItems;

        public CreateCategoryCommand(string categoryName, string? description, int? parentCategoryId, List<CategoryItemDTO>? categoryItems)
        {
            _categoryItems = categoryItems ?? new List<CategoryItemDTO>();
            CategoryName = categoryName;
            Description = description;
            ParentCategoryId = parentCategoryId;
        }

        public string CategoryName { get; private set; }
        public string? Description { get; private set; }
        public int? ParentCategoryId { get; private set; }
        public IEnumerable<CategoryItemDTO> CategoryItems => _categoryItems;
    }
}