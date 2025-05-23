using BuildingBlocks.Contracts.Products;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using ProductService.Domain.Abstractions.Repositories;
using ProductService.Domain.Entities;

namespace ProductService.Infracstructure.Consumers
{
    public class ProductInfoConsumer : IConsumer<GetProductInfoRequest>
    {
        private readonly IProductRepository _productRepository;

        public ProductInfoConsumer(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task Consume(ConsumeContext<GetProductInfoRequest> context)
        {
            int productId = context.Message.ProductId;

            Product? product = await _productRepository.AsQueryable().AsNoTracking().Include(p => p.ProductImages)
                .FirstOrDefaultAsync(p => p.Id == productId);

            if (product is null)
            {
                ProductInfoResponse notFoundResponse = new(0, string.Empty, null, 0, 0, 0);
                await context.RespondAsync(notFoundResponse);
                return;
            }

            string? mainImageUrl = product.ProductImages.FirstOrDefault(img => img.IsMain.HasValue && img.IsMain.Value)
                ?.ImageUrl;

            ProductInfoResponse response = new(product.Id, product.ProductName, mainImageUrl, product.Price,
                product.Discount ?? 0, product.Stock);

            await context.RespondAsync(response);
        }
    }
}