using BuildingBlocks.Abstractions.Repository;
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

        List<int> productIds = orderCancel.OrderItems.Select(item => item.ProductId).Distinct().ToList();

        List<Product> products = await _productRepository.AsQueryable().Where(p => productIds.Contains(p.Id))
            .ToListAsync(context.CancellationToken);

        foreach (OrderItemDTO item in orderCancel.OrderItems)
        {
            Product? product = products.FirstOrDefault(p => p.Id == item.ProductId);
            if (product is null)
                continue;

            product.UpdateStock(item.Quantity);
        }

        await _unitOfWork.SaveChangesAsync(context.CancellationToken);
    }
}