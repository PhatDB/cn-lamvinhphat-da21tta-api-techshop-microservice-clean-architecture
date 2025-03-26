using BuildingBlocks.Abstractions.Repository;
using BuildingBlocks.Contracts.Products;
using BuildingBlocks.Results;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using ProductService.Domain.Abstractions.Repositories;
using ProductService.Domain.Entities;

namespace ProductService.Infracstructure.Consumers
{
    public class UpdateProductStockConsumer : IConsumer<UpdateProductStock>
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProductStockConsumer(IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }


        public async Task Consume(ConsumeContext<UpdateProductStock> context)
        {
            UpdateProductStock message = context.Message;

            Result<Product> productResult =
                await _productRepository.AsQueryable().AsNoTracking().Include(p => p.Inventory).Where(p => p.Id == message.ProductId).FirstOrDefaultAsync();

            if (productResult.IsFailure)
                return;

            Product product = productResult.Value;

            Result updateResult = product.UpdateStock(message.Quantity);
            if (updateResult.IsFailure)
                return;

            await _productRepository.UpdateAsync(product);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}