using AutoMapper;
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
        private readonly IMapper _mapper;

        public GetCartByUserIdQueryHandler(ICartRepository cartRepository, ICartService cartService, IMapper mapper)
        {
            _cartRepository = cartRepository;
            _cartService = cartService;
            _mapper = mapper;
        }

        public async Task<Result<CartDTO>> Handle(GetCartByUserIdQuery request, CancellationToken cancellationToken)
        {
            Domain.Entities.Cart? cart = await _cartRepository.AsQueryable().Include(c => c.CartItems).FirstOrDefaultAsync(c => c.UserId == request.UserId, cancellationToken);

            if (cart == null) return Result.Failure<CartDTO>(Error.NotFound("Cart.NotFound", "Cart not found"));

            CartDTO cartDTO = new() { CartId = cart.Id, UserId = cart.UserId, CartItems = new List<ProductDTO>() };

            IEnumerable<Task<ProductDTO?>> productRequests = cart.CartItems.Select(async item =>
            {
                Result<ProductInfoResponse> productResult = await _cartService.GetProductInfo(item.ProductId);
                if (productResult.IsSuccess)
                {
                    ProductDTO? productDTO = _mapper.Map<ProductDTO>(productResult.Value);
                    productDTO.Quantity = item.Quantity;
                    return productDTO;
                }

                return null;
            });

            ProductDTO[] products = await Task.WhenAll(productRequests);

            List<ProductDTO> validProducts = products.Where(p => p != null).ToList();
            cartDTO.CartItems.AddRange(validProducts);

            if (!cartDTO.CartItems.Any()) return Result.Failure<CartDTO>(Error.NotFound("Product.NotFound", "No products found in the cart"));

            cartDTO.TotalPrice = cartDTO.CartItems.Sum(p => p.Price * p.Quantity);

            return Result.Success(cartDTO);
        }
    }
}