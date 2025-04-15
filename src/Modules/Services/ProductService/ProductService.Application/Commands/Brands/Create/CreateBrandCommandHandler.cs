using BuildingBlocks.Abstractions.Repository;
using BuildingBlocks.CQRS;
using BuildingBlocks.Results;
using Microsoft.EntityFrameworkCore;
using ProductService.Domain.Abstractions.Repositories;
using ProductService.Domain.Entities;
using ProductService.Domain.Errors;

namespace ProductService.Application.Commands.Brands.Create
{
    public class CreateBrandCommandHandler : ICommandHandler<CreateBrandCommand, int>
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateBrandCommandHandler(IBrandRepository brandRepository, IUnitOfWork unitOfWork)
        {
            _brandRepository = brandRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<int>> Handle(CreateBrandCommand request, CancellationToken cancellationToken)
        {
            Brand? existingBrand = await _brandRepository.AsQueryable()
                .FirstOrDefaultAsync(b => b.BrandName == request.BrandName, cancellationToken);

            if (existingBrand != null)
                return Result.Failure<int>(BrandError.BrandAlreadyExists);

            Result<Brand> brand = Brand.Create(request.BrandName, request.Description);

            if (brand.IsFailure)
                return Result.Failure<int>(brand.Error);


            await _brandRepository.AddAsync(brand.Value, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(brand.Value.Id);
        }
    }
}