/*using BuildingBlocks.Contracts.Products;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using ProductService.Domain.Abstractions.Repositories;
using ProductService.Domain.Entities;

namespace ProductService.Infracstructure.Consumers
{
    public class ProductInfoConsumer : IConsumer<GetProductInfo>
    {
        private readonly IProductRepository _productRepository;

        public ProductInfoConsumer(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task Consume(ConsumeContext<GetProductInfo> context)
        {
            Product? product = await _productRepository.AsQueryable().AsNoTracking().Include(p => p.ProductImages).Include(p => p.Inventory)
                .Where(p => p.Id == context.Message.ProductId).FirstOrDefaultAsync();

            if (product == null)
                await context.RespondAsync(new ProductInfoResponse(0, "Product Not Found", 0, string.Empty, "No description available", 0));
            else
                await context.RespondAsync(new ProductInfoResponse(product.Id, product.Name, product.Price, product.ProductImages.FirstOrDefault()?.ImageUrl ?? string.Empty,
                    product.Description, product.Inventory?.StockQuantity ?? 0));
        }
    }
}*/

