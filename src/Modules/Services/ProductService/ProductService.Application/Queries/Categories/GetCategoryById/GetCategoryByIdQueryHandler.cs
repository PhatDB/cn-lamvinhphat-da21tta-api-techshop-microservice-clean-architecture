using BuildingBlocks.CQRS;
using BuildingBlocks.Results;
using Microsoft.EntityFrameworkCore;
using ProductService.Domain.Abstractions.Repositories;
using ProductService.Domain.Entities;

namespace ProductService.Application.Queries.Categories.GetCategoryById
{
    public class GetCategoryByIdQueryHandler : IQueryHandler<GetCategoryByIdQuery, Category>
    {
        private readonly ICategoryRepository _categoryRepository;

        public GetCategoryByIdQueryHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<Result<Category>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            Result<Category> categories = await _categoryRepository.AsQueryable().AsNoTracking()
                .Where(c => c.Id == request.Id && c.IsActive).Include(c => c.Subcategories)
                .FirstOrDefaultAsync(cancellationToken);

            return Result.Success(categories.Value);
        }
    }
}