using BuildingBlocks.CQRS;
using BuildingBlocks.Results;
using Microsoft.EntityFrameworkCore;
using ProductService.Domain.Abstractions.Repositories;
using ProductService.Domain.Entities;

namespace ProductService.Application.Queries.Categories.GetAllCategory
{
    public class GetAllCategoryQueryHandler : IQueryHandler<GetAllCategoryQuery, List<Category>>
    {
        private readonly ICategoryRepository _categoryRepository;

        public GetAllCategoryQueryHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<Result<List<Category>>> Handle(
            GetAllCategoryQuery request, CancellationToken cancellationToken)
        {
            Result<List<Category>> categories = await _categoryRepository.AsQueryable().AsNoTracking()
                .Where(c => c.ParentId == null && c.IsActive).Include(c => c.Subcategories)
                .ToListAsync(cancellationToken);

            return Result.Success(categories.Value);
        }
    }
}