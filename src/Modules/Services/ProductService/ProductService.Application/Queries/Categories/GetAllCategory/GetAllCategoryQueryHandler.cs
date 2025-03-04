using BuildingBlocks.CQRS;
using BuildingBlocks.Results;
using ProductService.Domain.Abstractions.Repositories;
using ProductService.Domain.Entities;

namespace ProductService.Application.Queries.Categories.GetAllCategory
{
    public class
        GetAllCategoryQueryHandler : IQueryHandler<GetAllCategoryQuery, List<Category>>
    {
        private readonly ICategoryRepository _categoryRepository;

        public GetAllCategoryQueryHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<Result<List<Category>>> Handle(
            GetAllCategoryQuery request, CancellationToken cancellationToken)
        {
            Result<List<Category>> categories =
                await _categoryRepository.GetAllAsync(cancellationToken);

            return Result.Success(categories.Value);
        }
    }
}