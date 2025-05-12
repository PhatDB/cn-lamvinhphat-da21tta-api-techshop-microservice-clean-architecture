using BuildingBlocks.Abstractions.Extensions;
using BuildingBlocks.Abstractions.Repository;
using BuildingBlocks.CQRS;
using BuildingBlocks.Enumerations;
using BuildingBlocks.Results;
using ProductService.Domain.Abstractions.Repositories;
using ProductService.Domain.Entities;

namespace ProductService.Application.Commands.Categories.Create
{
    public class CreateCategoryCommandHandler : ICommandHandler<CreateCategoryCommand, int>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IFileService _fileService;
        private readonly IUnitOfWork _unitOfWork;

        public CreateCategoryCommandHandler(
            ICategoryRepository categoryRepository, IFileService fileService, IUnitOfWork unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _fileService = fileService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<int>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            string imageUrl = await _fileService.UploadFile(request.ImageContent, AssetType.CATEGORY_IMAGE);

            Result<Category> categoryResult =
                Category.Create(request.Name, request.Description, imageUrl, request.ParentId);

            if (categoryResult.IsFailure)
                return Result.Failure<int>(categoryResult.Error);

            Category category = categoryResult.Value;

            await _categoryRepository.AddAsync(category, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(category.Id);
        }
    }
}