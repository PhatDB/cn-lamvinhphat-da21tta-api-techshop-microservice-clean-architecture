using BuildingBlocks.CQRS;
using BuildingBlocks.Results;
using Microsoft.EntityFrameworkCore;
using ProductService.Domain.Abstractions.Repositories;
using ProductService.Domain.Entities;

namespace ProductService.Application.Queries.Brands.GetAllActiveBrands
{
    public class GetAllActiveBrandQueryHandler : IQueryHandler<GetAllActiveBrandQuery, List<Brand>>
    {
        private readonly IBrandRepository _brandRepository;

        public GetAllActiveBrandQueryHandler(IBrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
        }

        public async Task<Result<List<Brand>>> Handle(
            GetAllActiveBrandQuery request, CancellationToken cancellationToken)
        {
            Result<List<Brand>> brands = await _brandRepository.AsQueryable().Where(b => b.IsActive).AsNoTracking()
                .ToListAsync(cancellationToken);

            return Result.Success(brands.Value);
        }
    }
}