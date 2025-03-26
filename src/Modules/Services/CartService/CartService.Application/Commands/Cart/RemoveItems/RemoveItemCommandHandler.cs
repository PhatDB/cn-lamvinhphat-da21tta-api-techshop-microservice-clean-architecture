using BuildingBlocks.Abstractions.Repository;
using BuildingBlocks.Contracts.Products;
using BuildingBlocks.CQRS;
using BuildingBlocks.Error;
using BuildingBlocks.Results;
using CartService.Domain.Abstractions.Repositories;
using CartService.Domain.Entities;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace CartService.Application.Commands.Cart.RemoveItems
{
    public class RemoveItemCommandHandler : ICommandHandler<RemoveItemCommand>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IUnitOfWork _unitOfWork;

        public RemoveItemCommandHandler(ICartRepository cartRepository, IUnitOfWork unitOfWork, IPublishEndpoint publishEndpoint)
        {
            _cartRepository = cartRepository;
            _unitOfWork = unitOfWork;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<Result> Handle(RemoveItemCommand request, CancellationToken cancellationToken)
        {
            Domain.Entities.Cart? cart = await _cartRepository.AsQueryable().AsNoTracking().Include(c => c.CartItems).Where(c => c.Id == request.CartId)
                .FirstOrDefaultAsync(cancellationToken);

            if (cart == null)
                return Result.Failure(Error.NotFound("Cart.NotFound", "Cart not found"));

            CartItem? existingItem = cart.CartItems.FirstOrDefault(i => i.ProductId == request.ProductId);
            if (existingItem == null)
                return Result.Failure(Error.NotFound("Cart.ItemNotFound", "Item not found in cart"));

            Result removeResult = cart.RemoveItem(request.ProductId);
            if (removeResult.IsFailure)
                return removeResult;

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            UpdateProductStock updateStockEvent = new(request.ProductId, existingItem.Quantity);
            await _publishEndpoint.Publish(updateStockEvent, cancellationToken);

            return Result.Success();
        }
    }
}