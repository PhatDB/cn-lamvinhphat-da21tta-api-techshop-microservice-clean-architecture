using AutoMapper;
using BuildingBlocks.CQRS;
using BuildingBlocks.Results;
using Microsoft.EntityFrameworkCore;
using ProductService.Application.DTOs;
using ProductService.Domain.Abstractions.Repositories;
using ProductService.Domain.Entities;

namespace ProductService.Application.Queries.Products.GetProductByCategoryId
{
    public class GetProductByCategoryIdQueryHandler : IQueryHandler<
        GetProductByCategoryIdQuery, List<GetAllProductDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;

        public GetProductByCategoryIdQueryHandler(
            IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<Result<List<GetAllProductDTO>>> Handle(
            GetProductByCategoryIdQuery request, CancellationToken cancellationToken)
        {
            List<Product> products = await _productRepository.AsQueryable()
                .Where(p => p.CategoryId == request.CategoryId)
                .Include(p => p.ProductImages).Include(p => p.ProductColors)
                .ThenInclude(pc => pc.Color).AsNoTracking()
                .ToListAsync(cancellationToken);

            List<GetAllProductDTO> productDTOs =
                _mapper.Map<List<GetAllProductDTO>>(products);

            return Result.Success(productDTOs);
        }
    }
}