using BuildingBlocks.Contracts.Products;
using BuildingBlocks.CQRS;
using BuildingBlocks.Error;
using BuildingBlocks.Results;
using CartService.Application.Abstractions;
using CartService.Application.DTOs;
using CartService.Domain.Abstractions.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CartService.Application.Queries.Cart
{
    public class GetCartByUserIdQueryHandler : IQueryHandler<GetCartByUserIdQuery, CartDTO>
    {
        private readonly ICartRepository _cartRepository;
        private readonly ICartService _cartService;

        public GetCartByUserIdQueryHandler(ICartRepository cartRepository, ICartService cartService)
        {
            _cartRepository = cartRepository;
            _cartService = cartService;
        }

        public async Task<Result<CartDTO>> Handle(GetCartByUserIdQuery request, CancellationToken cancellationToken)
        {
            Domain.Entities.Cart? cart = await _cartRepository.AsQueryable().Include(c => c.CartItems).FirstOrDefaultAsync(c => c.UserId == request.UserId, cancellationToken);

            if (cart == null)
                return Result.Failure<CartDTO>(Error.NotFound("Cart.NotFound", "Cart not found"));

            CartDTO cartDTO = new() { CartId = cart.Id, UserId = cart.UserId, CartItems = new List<ProductDTO>() };

            IEnumerable<Task<ProductDTO?>> productRequests = cart.CartItems.Select(async item =>
            {
                Result<ProductInfoResponse> productResult = await _cartService.GetProductInfo(item.ProductId);
                if (productResult.IsSuccess)
                    return new ProductDTO
                    {
                        ProductId = productResult.Value.ProductId,
                        Name = productResult.Value.Name,
                        Description = productResult.Value.Description,
                        Price = productResult.Value.Price,
                        Quantity = item.Quantity,
                        ImageUrl = productResult.Value.ImageUrl
                    };

                return null;
            });

            ProductDTO?[] products = await Task.WhenAll(productRequests);

            cartDTO.CartItems.AddRange(products.Where(p => p != null));

            if (!cartDTO.CartItems.Any())
                return Result.Failure<CartDTO>(Error.NotFound("Product.NotFound", "No products found in the cart"));

            cartDTO.TotalPrice = products.Sum(p => p.Price) * products.Sum(p => p.Quantity);

            return Result.Success(cartDTO);
        }
    }
}