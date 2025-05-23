using BuildingBlocks.Abstractions.Repository;
using BuildingBlocks.Contracts.Products;
using BuildingBlocks.CQRS;
using BuildingBlocks.Error;
using BuildingBlocks.Results;
using CartService.Application.Abstractions;
using CartService.Domain.Abstractions.Repositories;
using MassTransit;

namespace CartService.Application.Commands.Cart.AddToCarts
{
    public class AddToCartCommandHandler : ICommandHandler<AddToCartCommand, int>
    {
        private readonly ICartRepository _cartRepository;
        private readonly ICartService _cartService;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IUnitOfWork _unitOfWork;

        public AddToCartCommandHandler(
            ICartRepository cartRepository, IUnitOfWork unitOfWork, ICartService cartService,
            IPublishEndpoint publishEndpoint)
        {
            _cartRepository = cartRepository;
            _unitOfWork = unitOfWork;
            _cartService = cartService;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<Result<int>> Handle(AddToCartCommand request, CancellationToken cancellationToken)
        {
            Result customerCheck = await _cartService.IsCustomerExist(request.CustomerId);
            if (customerCheck.IsFailure)
                return Result.Failure<int>(customerCheck.Error);

            Result<ProductInfoResponse> productResult = await _cartService.GetProductInfo(request.ProductId);
            if (productResult.IsFailure)
                return Result.Failure<int>(productResult.Error);

            ProductInfoResponse product = productResult.Value;
            decimal finalPrice = product.Price - product.Price * product.Discount / 100;

            if (request.Quantity > product.Stock)
                return Result.Failure<int>(Error.Validation("Product.OutOfStock", "Product is out of stock."));

            Result<Domain.Entities.Cart> cartResult =
                await _cartRepository.GetUserCartAsync(request.CustomerId, cancellationToken);

            Domain.Entities.Cart cart;
            if (cartResult.IsFailure)
            {
                Result<Domain.Entities.Cart> cartCreateResult = Domain.Entities.Cart.Create(request.CustomerId);
                if (cartCreateResult.IsFailure)
                    return Result.Failure<int>(cartCreateResult.Error);

                cart = cartCreateResult.Value;
                Result addResult = cart.AddItem(request.ProductId, request.Quantity, finalPrice);
                if (addResult.IsFailure)
                    return Result.Failure<int>(addResult.Error);

                await _cartRepository.AddAsync(cart, cancellationToken);

                await _publishEndpoint.Publish(new UpdateProductStock(request.ProductId, -request.Quantity),
                    cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                return Result.Success(cart.Id);
            }

            cart = cartResult.Value;

            int oldQuantity = cart.CartItems.FirstOrDefault(i => i.ProductId == request.ProductId)?.Quantity ?? 0;
            int newQuantity = request.Quantity;
            int stockQuantityChange = oldQuantity - newQuantity;

            Result updateResult = cart.AddOrUpdateItem(request.ProductId, newQuantity, finalPrice);
            if (updateResult.IsFailure)
                return Result.Failure<int>(updateResult.Error);

            if (stockQuantityChange != 0)
                await _publishEndpoint.Publish(new UpdateProductStock(request.ProductId, stockQuantityChange),
                    cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success(cart.Id);
        }
    }
}