using BuildingBlocks.Abstractions.Repository;
using BuildingBlocks.CQRS;
using BuildingBlocks.Results;
using ProductService.Domain.Abstractions.Repositories;
using ProductService.Domain.Entities;
using ProductService.Domain.Errors;

namespace ProductService.Application.Commands.Brands.Delete
{
    public class DeleteBrandCommandHandler : ICommandHandler<DeleteBrandCommand>
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteBrandCommandHandler(IBrandRepository brandRepository, IUnitOfWork unitOfWork)
        {
            _brandRepository = brandRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(DeleteBrandCommand request, CancellationToken cancellationToken)
        {
            Result<Brand>? brandResult = await _brandRepository.GetByIdAsync(request.BrandId, cancellationToken);

            Brand? brand = brandResult.Value;

            if (brand == null)
                return Result.Failure(BrandError.BrandNotFound);

            brand.DeleteBrand();

            await _brandRepository.UpdateAsync(brand, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}