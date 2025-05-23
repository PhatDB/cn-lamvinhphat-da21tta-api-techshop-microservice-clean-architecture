using BuildingBlocks.Abstractions.Repository;
using BuildingBlocks.Contracts.Products;
using BuildingBlocks.CQRS;
using BuildingBlocks.Error;
using BuildingBlocks.Results;
using CartService.Domain.Abstractions.Repositories;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace CartService.Application.Commands.Cart.ClearCart
{
    public class ClearCartCommandHandler : ICommandHandler<ClearCartCommand>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IUnitOfWork _unitOfWork;

        public ClearCartCommandHandler(
            ICartRepository cartRepository, IUnitOfWork unitOfWork, IPublishEndpoint publishEndpoint)
        {
            _cartRepository = cartRepository;
            _unitOfWork = unitOfWork;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<Result> Handle(ClearCartCommand request, CancellationToken cancellationToken)
        {
            Domain.Entities.Cart? cart = await _cartRepository.AsQueryable().Include(x => x.CartItems)
                .FirstOrDefaultAsync(c => c.Id == request.CartId, cancellationToken);

            if (cart is null)
                return Result.Failure(Error.NotFound("Cart.NotFound", "Cart not found"));

            List<StockUpdateItem> stockItems = cart.CartItems
                .Select(item => new StockUpdateItem(item.ProductId, item.Quantity)).ToList();

            cart.ClearCart();

            if (stockItems.Any())
                await _publishEndpoint.Publish(new BulkUpdateProductStock(stockItems), cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}