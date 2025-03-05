using BuildingBlocks.Abstractions.Repository;
using BuildingBlocks.CQRS;
using BuildingBlocks.Results;
using ProductService.Domain.Abstractions.Repositories;
using ProductService.Domain.Entities;
using ProductService.Domain.Errors;

namespace ProductService.Application.Commands.Categories.Update
{
    public class UpdateCategoryCommandHandler : ICommandHandler<UpdateCategoryCommand>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCategoryCommandHandler(
            ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(
            UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            Result<Category> categoryResult =
                await _categoryRepository.GetByIdAsync(request.Id, cancellationToken);

            if (categoryResult.IsFailure)
                return Result.Failure(CategoryError.CategoryNotFound);

            Category category = categoryResult.Value;

            Result updateResult = category.Update(request.Name, request.Description,
                request.IsActive);
            if (updateResult.IsFailure)
                return updateResult;

            await _categoryRepository.UpdateAsync(category, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}