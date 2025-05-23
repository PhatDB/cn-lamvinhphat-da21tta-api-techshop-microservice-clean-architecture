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
    public class GetCartByCustomerIdQueryHandler : IQueryHandler<GetCartByCustomerIdQuery, CartDTO>
    {
        private readonly ICartRepository _cartRepository;
        private readonly ICartService _cartService;
        private readonly IMapper _mapper;

        public GetCartByCustomerIdQueryHandler(ICartRepository cartRepository, ICartService cartService, IMapper mapper)
        {
            _cartRepository = cartRepository;
            _cartService = cartService;
            _mapper = mapper;
        }

        public async Task<Result<CartDTO>> Handle(GetCartByCustomerIdQuery request, CancellationToken cancellationToken)
        {
            Domain.Entities.Cart? cart = await _cartRepository.AsQueryable().Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.CustomerId == request.CustomerId, cancellationToken);

            if (cart is null)
                return Result.Failure<CartDTO>(Error.NotFound("Cart.NotFound", "Cart not found"));

            IEnumerable<Task<ProductDTO>> productTasks = cart.CartItems.Select(async item =>
            {
                Result<ProductInfoResponse> productResult = await _cartService.GetProductInfo(item.ProductId);
                if (productResult.IsSuccess)
                {
                    ProductDTO? productDto = _mapper.Map<ProductDTO>(productResult.Value);
                    productDto.Quantity = item.Quantity;
                    return productDto;
                }

                return null;
            });

            ProductDTO[] productResults = await Task.WhenAll(productTasks);
            List<ProductDTO> validProducts = productResults.Where(p => p is not null).ToList()!;

            if (!validProducts.Any())
                return Result.Failure<CartDTO>(Error.NotFound("Product.NotFound", "No products found in the cart"));

            CartDTO cartDTO = new()
            {
                CartId = cart.Id,
                CustomerId = cart.CustomerId,
                CartItems = validProducts,
                TotalPrice = validProducts.Sum(p => p.DiscountPrice * p.Quantity)
            };

            return Result.Success(cartDTO);
        }
    }
}