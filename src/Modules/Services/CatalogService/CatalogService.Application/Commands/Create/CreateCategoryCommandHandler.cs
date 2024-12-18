using BuildingBlocks.Abstractions.Repository;
using BuildingBlocks.CQRS;
using BuildingBlocks.Results;
using CatalogService.Domain.Abstractions.Repositories;
using CatalogService.Domain.Entities;

namespace CatalogService.Application.Commands.Create
{
    public class CreateCategoryCommandHandler: ICommandHandler<CreateCategoryCommand, int>
    {
        public CreateCategoryCommandHandler(IUnitOfWork unitOfWork, ICategoryRepo categoryRepo)
        {
            _unitOfWork = unitOfWork;
            _categoryRepo = categoryRepo;
        }

        private readonly IUnitOfWork _unitOfWork;
        private readonly ICategoryRepo _categoryRepo;
        
        
        public async Task<Result<int>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = Category.Create(request.CategoryName , request.Description, request.ParentCategoryId);

            foreach (var item in request.CategoryItems)
            {
                category.AddCategoryItem(item.ProductId);
            }
            
            _categoryRepo.AddAsync(category);
            
            return await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}