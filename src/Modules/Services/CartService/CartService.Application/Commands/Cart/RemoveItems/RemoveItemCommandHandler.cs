using BuildingBlocks.Abstractions.Repository;
using BuildingBlocks.CQRS;
using BuildingBlocks.Error;
using BuildingBlocks.Results;
using CartService.Domain.Abstractions.Repositories;
using CartService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CartService.Application.Commands.Cart.RemoveItems
{
    public class RemoveItemCommandHandler : ICommandHandler<RemoveItemCommand>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RemoveItemCommandHandler(ICartRepository cartRepository, IUnitOfWork unitOfWork)
        {
            _cartRepository = cartRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(RemoveItemCommand request, CancellationToken cancellationToken)
        {
            Domain.Entities.Cart? cart = await _cartRepository.AsQueryable().Include(c => c.CartItems)
                .Where(c => c.Id == request.CartId).FirstOrDefaultAsync(cancellationToken);

            if (cart == null)
                return Result.Failure(Error.NotFound("Cart.NotFound", "Cart not found"));

            CartItem? existingItem = cart.CartItems.FirstOrDefault(i => i.ProductId == request.ProductId);
            if (existingItem == null)
                return Result.Failure(Error.NotFound("Cart.ItemNotFound", "Item not found in cart"));

            Result removeResult = cart.RemoveItem(request.ProductId);
            if (removeResult.IsFailure)
                return removeResult;

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}