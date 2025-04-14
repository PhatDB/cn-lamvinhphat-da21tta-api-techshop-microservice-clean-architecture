/*using BuildingBlocks.Abstractions.Repository;
using BuildingBlocks.Contracts.Orders;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using ProductService.Domain.Abstractions.Repositories;
using ProductService.Domain.Entities;

public class OrderCancelConsumer : IConsumer<OrderCancel>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public OrderCancelConsumer(IProductRepository productRepository, IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Consume(ConsumeContext<OrderCancel> context)
    {
        OrderCancel orderCancel = context.Message;

        List<int> productIds = orderCancel.OrderItems.Select(item => item.ProductId).ToList();

        List<Product> products = await _productRepository.AsQueryable().Include(p => p.Inventory).Where(p => productIds.Contains(p.Id)).ToListAsync();

        List<Product> updatedProducts = orderCancel.OrderItems.Select(item =>
        {
            Product? product = products.FirstOrDefault(p => p.Id == item.ProductId);
            if (product != null && product.Inventory != null) product.UpdateStock(item.Quantity);

            return product;
        }).Where(p => p != null).ToList();

        await _unitOfWork.SaveChangesAsync();
    }
}*/

