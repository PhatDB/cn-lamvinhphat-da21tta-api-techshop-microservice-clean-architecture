using BuildingBlocks.CQRS;

namespace CatalogService.Application.Commands.Delete
{
    public class DeleteCategoryCommand : ICommand
    {
        public DeleteCategoryCommand(int categoryId)
        {
            CategoryId = categoryId;
        }
        
        public int CategoryId { get; private set; }
    }
}