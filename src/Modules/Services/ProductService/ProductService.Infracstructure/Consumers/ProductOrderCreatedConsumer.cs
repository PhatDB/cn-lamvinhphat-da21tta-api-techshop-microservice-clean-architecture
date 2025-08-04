using BuildingBlocks.Abstractions.Repository;
using BuildingBlocks.Contracts.Orders;
using BuildingBlocks.Results;
using MassTransit;
using ProductService.Domain.Abstractions.Repositories;
using ProductService.Domain.Entities;

namespace ProductService.Infracstructure.Consumers
{
    public class ProductOrderCreatedConsumer : IConsumer<OrderCreated>
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProductOrderCreatedConsumer(IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Consume(ConsumeContext<OrderCreated> context)
        {
            foreach (OrderItemInfo item in context.Message.Items)
            {
                Result<Product> productResult = await _productRepository.GetByIdAsync(item.ProductId);

                Product? product = productResult.Value;

                if (product != null)
                {
                    product.UpdateSoldQuantity(item.Quantity);
                    product.UpdateStock(-item.Quantity);
                }
            }

            await _unitOfWork.SaveChangesAsync();
        }
    }
}