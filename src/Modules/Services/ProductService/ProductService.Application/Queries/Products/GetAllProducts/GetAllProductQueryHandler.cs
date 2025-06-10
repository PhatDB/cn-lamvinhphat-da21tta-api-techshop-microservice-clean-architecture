using AutoMapper;
using BuildingBlocks.CQRS;
using BuildingBlocks.Results;
using Microsoft.EntityFrameworkCore;
using ProductService.Application.DTOs;
using ProductService.Domain.Abstractions.Repositories;
using ProductService.Domain.Entities;

namespace ProductService.Application.Queries.Products.GetAllProducts
{
    public class GetAllProductQueryHandler : IQueryHandler<GetAllProductQuery, List<GetAllProductDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;

        public GetAllProductQueryHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<Result<List<GetAllProductDTO>>> Handle(
            GetAllProductQuery request, CancellationToken cancellationToken)
        {
            List<Product> query = await _productRepository.AsQueryable().Include(p => p.ProductImages)
                .Include(p => p.ProductSpecs).AsNoTracking().ToListAsync();

            List<GetAllProductDTO>? items = _mapper.Map<List<GetAllProductDTO>>(query);

            return Result.Success(items);
        }
    }
}