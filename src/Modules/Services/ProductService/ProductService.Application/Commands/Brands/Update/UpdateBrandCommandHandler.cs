using BuildingBlocks.Abstractions.Repository;
using BuildingBlocks.CQRS;
using BuildingBlocks.Results;
using ProductService.Domain.Abstractions.Repositories;
using ProductService.Domain.Entities;
using ProductService.Domain.Errors;

namespace ProductService.Application.Commands.Brands.Update
{
    public class UpdateBrandCommandHandler : ICommandHandler<UpdateBrandCommand>
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateBrandCommandHandler(IBrandRepository brandRepository, IUnitOfWork unitOfWork)
        {
            _brandRepository = brandRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(UpdateBrandCommand request, CancellationToken cancellationToken)
        {
            Result<Brand>? brandResult = await _brandRepository.GetByIdAsync(request.BrandId, cancellationToken);

            Brand? brand = brandResult.Value;

            if (brand == null)
                return Result.Failure(BrandError.BrandNotFound);

            Result updateResult = brand.UpdateBrand(request.BrandName, request.Description, request.IsActive);

            if (updateResult.IsFailure)
                return updateResult;

            await _brandRepository.UpdateAsync(brand, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}