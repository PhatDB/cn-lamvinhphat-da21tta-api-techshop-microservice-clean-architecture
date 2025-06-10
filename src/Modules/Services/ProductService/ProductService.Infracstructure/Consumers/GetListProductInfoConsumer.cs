using BuildingBlocks.Contracts.Products;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using ProductService.Domain.Abstractions.Repositories;

namespace ProductService.Infracstructure.Consumers
{
    public class GetListProductInfoConsumer : IConsumer<GetListProductInfoRequest>
    {
        private readonly IProductRepository _productRepository;

        public GetListProductInfoConsumer(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task Consume(ConsumeContext<GetListProductInfoRequest> context)
        {
            List<int> productIds = context.Message.ProductIds;

            List<ProductInfoResponse> products = await _productRepository.AsQueryable()
                .Where(p => productIds.Contains(p.Id)).Select(p => new ProductInfoResponse(p.Id, p.ProductName,
                    p.ProductImages.FirstOrDefault(img => img.IsMain.Value).ImageUrl, p.Price, p.Discount.Value,
                    p.Stock)).ToListAsync();

            await context.RespondAsync(new GetListProductInfoResponse(products));
        }
    }
}