using BuildingBlocks.CQRS;
using BuildingBlocks.Results;
using Microsoft.EntityFrameworkCore;
using ProductService.Domain.Abstractions.Repositories;
using ProductService.Domain.Entities;

namespace ProductService.Application.Queries.Categories.GetAllCategoryAdmin
{
    public class GetAllCategoryAdminQueryHandler : IQueryHandler<GetAllCategoryAdminQuery, List<Category>>
    {
        private readonly ICategoryRepository _categoryRepository;

        public GetAllCategoryAdminQueryHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<Result<List<Category>>> Handle(
            GetAllCategoryAdminQuery request, CancellationToken cancellationToken)
        {
            Result<List<Category>> categories = await _categoryRepository.AsQueryable().AsNoTracking()
                .Where(c => c.IsActive).Include(c => c.Subcategories).ToListAsync(cancellationToken);

            return Result.Success(categories.Value);
        }
    }
}