using BuildingBlocks.Abstractions.Extensions;
using BuildingBlocks.Abstractions.Repository;
using BuildingBlocks.CQRS;
using BuildingBlocks.Enumerations;
using BuildingBlocks.Error;
using BuildingBlocks.Results;
using ProductService.Domain.Abstractions.Repositories;
using ProductService.Domain.Entities;
using ProductService.Domain.Errors;

namespace ProductService.Application.Commands.Categories.Update
{
    public class UpdateCategoryCommandHandler : ICommandHandler<UpdateCategoryCommand>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IFileService _fileService;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCategoryCommandHandler(
            ICategoryRepository categoryRepository, IFileService fileService, IUnitOfWork unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _fileService = fileService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            Result<Category> categoryResult = await _categoryRepository.GetByIdAsync(request.Id, cancellationToken);

            if (categoryResult.IsFailure)
                return Result.Failure(CategoryError.CategoryNotFound);

            Category category = categoryResult.Value;

            string imageUrl = category.ImageUrl;

            if (request.ImageContent != null)
            {
                if (!string.IsNullOrWhiteSpace(category.ImageUrl))
                {
                    bool deleted = await _fileService.DeleteFile(category.ImageUrl);
                    if (!deleted)
                        return Result.Failure(Error.Problem("FailToDelete", "Cannot delete old image."));
                }

                imageUrl = await _fileService.UploadFile(request.ImageContent, AssetType.CATEGORY_IMAGE);
            }

            Result updateResult = category.UpdateCategory(request.CategoryName, request.Description, imageUrl,
                request.ParentId, request.IsActive);

            if (updateResult.IsFailure)
                return updateResult;

            await _categoryRepository.UpdateAsync(category, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}