using BuildingBlocks.Abstractions.Repository;
using BuildingBlocks.Contracts.Products;
using BuildingBlocks.CQRS;
using BuildingBlocks.Results;
using CartService.Application.Abstractions;
using CartService.Domain.Abstractions.Repositories;

namespace CartService.Application.Commands.Cart.Create
{
    public class CreateCartCommandHandler : ICommandHandler<CreateCartCommand, int>
    {
        private readonly ICartRepository _cartRepository;
        private readonly ICartService _cartService;
        private readonly IUnitOfWork _unitOfWork;

        public CreateCartCommandHandler(ICartRepository cartRepository, IUnitOfWork unitOfWork, ICartService cartService)
        {
            _cartRepository = cartRepository;
            _unitOfWork = unitOfWork;
            _cartService = cartService;
        }

        public async Task<Result<int>> Handle(CreateCartCommand request, CancellationToken cancellationToken)
        {
            Result userResult = await _cartService.IsUserExist(request.UserId);
            if (userResult.IsFailure)
                return Result.Failure<int>(userResult.Error);

            Result<ProductInfoResponse> productResult = await _cartService.GetProductInfo(request.ProductId);
            if (productResult.IsFailure)
                return Result.Failure<int>(productResult.Error);

            ProductInfoResponse product = productResult.Value;

            Result<Domain.Entities.Cart> cartResult = await _cartRepository.GetUserCartAsync(request.UserId, cancellationToken);

            Domain.Entities.Cart cart = cartResult.Value;

            if (cart == null)
            {
                cart = Domain.Entities.Cart.Create(request.UserId).Value;

                Result addResult = cart.AddItem(request.ProductId, request.Quantity, product.Price);
                if (addResult.IsFailure)
                    return Result.Failure<int>(addResult.Error);

                await _cartRepository.AddAsync(cart, cancellationToken);
            }
            else
            {
                var updateResult = cart.AddOrUpdateItem(request.ProductId, request.Quantity, product.Price);
                if (updateResult.IsFailure)
                    return Result.Failure<int>(updateResult.Error);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success(cart.Id);
        }
    }
}