using BuildingBlocks.Abstractions.Repository;
using BuildingBlocks.Contracts.Products;
using BuildingBlocks.CQRS;
using BuildingBlocks.Error;
using BuildingBlocks.Results;
using CartService.Application.Abstractions;
using CartService.Domain.Abstractions.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CartService.Application.Commands.Cart.AddToCarts
{
    public class AddToCartCommandHandler : ICommandHandler<AddToCartCommand, int>
    {
        private readonly ICartRepository _cartRepository;
        private readonly ICartService _cartService;
        private readonly IUnitOfWork _unitOfWork;

        public AddToCartCommandHandler(ICartRepository cartRepository, IUnitOfWork unitOfWork, ICartService cartService)
        {
            _cartRepository = cartRepository;
            _unitOfWork = unitOfWork;
            _cartService = cartService;
        }

        public async Task<Result<int>> Handle(AddToCartCommand request, CancellationToken cancellationToken)
        {
            if (!request.CustomerId.HasValue && string.IsNullOrEmpty(request.SessionId))
                return Result.Failure<int>(Error.Validation("InvalidRequest",
                    "CustomerId or SessionId must be provided."));

            if (request.CustomerId.HasValue)
            {
                Result exists = await _cartService.IsCustomerExist(request.CustomerId.Value);
                if (exists.IsFailure)
                    return Result.Failure<int>(exists.Error);
            }

            Result<ProductInfoResponse> productRes = await _cartService.GetProductInfo(request.ProductId);
            if (productRes.IsFailure)
                return Result.Failure<int>(productRes.Error);

            ProductInfoResponse product = productRes.Value;
            decimal finalPrice = product.Price * (1 - product.Discount / 100);

            if (request.Quantity > product.Stock)
                return Result.Failure<int>(Error.Validation("Product.OutOfStock", "Product is out of stock."));

            Result<Domain.Entities.Cart> cartResult = request.CustomerId.HasValue
                ? await _cartRepository.GetCartAsync(request.CustomerId.Value, request.SessionId, cancellationToken)
                : await _cartRepository.AsQueryable().Include(c => c.CartItems)
                    .FirstOrDefaultAsync(c => c.SessionId == request.SessionId, cancellationToken).ContinueWith(
                        t => t.Result is null
                            ? Result.Failure<Domain.Entities.Cart>(Error.NotFound("Cart.NotFound", "Cart not found"))
                            : Result.Success(t.Result), cancellationToken);

            Domain.Entities.Cart cart;
            if (cartResult.IsFailure)
            {
                Result<Domain.Entities.Cart> createRes =
                    Domain.Entities.Cart.Create(request.CustomerId, request.SessionId);
                if (createRes.IsFailure)
                    return Result.Failure<int>(createRes.Error);

                cart = createRes.Value;
                await _cartRepository.AddAsync(cart, cancellationToken);
            }
            else
            {
                cart = cartResult.Value;
            }

            Result itemRes = cart.AddOrUpdateItem(request.ProductId, request.Quantity, finalPrice);
            if (itemRes.IsFailure)
                return Result.Failure<int>(itemRes.Error);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success(cart.Id);
        }
    }
}