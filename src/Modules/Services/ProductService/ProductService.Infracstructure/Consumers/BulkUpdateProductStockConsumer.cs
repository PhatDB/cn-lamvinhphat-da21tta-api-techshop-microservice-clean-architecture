using BuildingBlocks.Abstractions.Repository;
using BuildingBlocks.Contracts.Products;
using BuildingBlocks.Results;
using MassTransit;
using ProductService.Domain.Abstractions.Repositories;
using ProductService.Domain.Entities;

public class BulkUpdateProductStockConsumer : IConsumer<BulkUpdateProductStock>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public BulkUpdateProductStockConsumer(IProductRepository productRepository, IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Consume(ConsumeContext<BulkUpdateProductStock> context)
    {
        foreach (StockUpdateItem item in context.Message.Items)
        {
            Result<Product> productResult = await _productRepository.GetByIdAsync(item.ProductId);
            Product? product = productResult.Value;
            if (product != null) product.UpdateStock(item.Quantity);
        }

        await _unitOfWork.SaveChangesAsync();
    }
}