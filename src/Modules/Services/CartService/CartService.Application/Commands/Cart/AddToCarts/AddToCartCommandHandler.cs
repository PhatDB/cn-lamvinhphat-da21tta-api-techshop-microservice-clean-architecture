using BuildingBlocks.Abstractions.Repository;
using BuildingBlocks.Contracts.Products;
using BuildingBlocks.CQRS;
using BuildingBlocks.Error;
using BuildingBlocks.Results;
using CartService.Application.Abstractions;
using CartService.Domain.Abstractions.Repositories;
using CartService.Domain.Entities;
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
            Domain.Entities.Cart cart = cartResult.IsFailure ? (await CreateNewCart(request.UserId, product, request.Quantity, cancellationToken)).Value : cartResult.Value;

            int stockQuantityChange = CalculateStockChange(cart, request.ProductId, request.Quantity);

            Result updateResult = cart.AddOrUpdateItem(request.ProductId, product.Name, product.ImageUrl, request.Quantity, product.Price);
            if (updateResult.IsFailure)
                return Result.Failure<int>(updateResult.Error);

            await _publishEndpoint.Publish(new UpdateProductStock(product.ProductId, stockQuantityChange), cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(cart.Id);
        }

        private async Task<Result<Domain.Entities.Cart>> CreateNewCart(int userId, ProductInfoResponse product, int quantity, CancellationToken cancellationToken)
        {
            Result<Domain.Entities.Cart> cartCreateResult = Domain.Entities.Cart.Create(userId);
            if (cartCreateResult.IsFailure)
                return cartCreateResult;

            Domain.Entities.Cart cart = cartCreateResult.Value;
            Result addResult = cart.AddItem(product.ProductId, product.Name, product.ImageUrl, quantity, product.Price);
            if (addResult.IsFailure)
                return Result.Failure<Domain.Entities.Cart>(addResult.Error);

            await _cartRepository.AddAsync(cart, cancellationToken);
            return Result.Success(cart);
        }

        private int CalculateStockChange(Domain.Entities.Cart cart, int productId, int quantity)
        {
            CartItem? existingItem = cart.CartItems.FirstOrDefault(i => i.ProductId == productId);
            if (existingItem != null)
                return quantity - existingItem.Quantity;
            return -quantity;
        }
    }
}