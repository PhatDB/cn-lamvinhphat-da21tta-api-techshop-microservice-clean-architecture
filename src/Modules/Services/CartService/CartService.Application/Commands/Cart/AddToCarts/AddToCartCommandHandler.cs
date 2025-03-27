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

        public AddToCartCommandHandler(ICartRepository cartRepository, IUnitOfWork unitOfWork, ICartService cartService, IPublishEndpoint publishEndpoint)
        {
            _cartRepository = cartRepository;
            _unitOfWork = unitOfWork;
            _cartService = cartService;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<Result<int>> Handle(AddToCartCommand request, CancellationToken cancellationToken)
        {
            Result userCheck = await _cartService.IsUserExist(request.UserId);
            if (userCheck.IsFailure)
                return Result.Failure<int>(userCheck.Error);

            Result<ProductInfoResponse> productResult = await _cartService.GetProductInfo(request.ProductId);
            if (productResult.IsFailure)
                return Result.Failure<int>(productResult.Error);

            ProductInfoResponse product = productResult.Value;

            if (request.Quantity > product.StockQuantity)
                return Result.Failure<int>(Error.Validation("Product.OutOfStock", "Product is out of stock."));

            Result<Domain.Entities.Cart> cartResult = await _cartRepository.GetUserCartAsync(request.UserId, cancellationToken);

            Domain.Entities.Cart cart;

            if (cartResult.IsFailure)
            {
                Result<Domain.Entities.Cart> cartCreateResult = Domain.Entities.Cart.Create(request.UserId);
                if (cartCreateResult.IsFailure)
                    return Result.Failure<int>(cartCreateResult.Error);

                cart = cartCreateResult.Value;

                Result addResult = cart.AddItem(request.ProductId, product.Name, product.ImageUrl, request.Quantity, product.Price);
                if (addResult.IsFailure)
                    return Result.Failure<int>(addResult.Error);

                await _cartRepository.AddAsync(cart, cancellationToken);
            }
            else
            {
                cart = cartResult.Value;

                Result updateResult = cart.AddOrUpdateItem(request.ProductId, product.Name, product.ImageUrl, request.Quantity, product.Price);
                if (updateResult.IsFailure)
                    return Result.Failure<int>(updateResult.Error);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _publishEndpoint.Publish(new UpdateProductStock(product.ProductId, -request.Quantity), cancellationToken);

            return Result.Success(cart.Id);
        }
    }
}