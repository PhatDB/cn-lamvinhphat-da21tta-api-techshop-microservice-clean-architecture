using AutoMapper;
using BuildingBlocks.Contracts.Products;
using CartService.Application.DTOs;

namespace CartService.Application.MappingProfiles
{
    public class CartMappingProfile : Profile
    {
        public CartMappingProfile()
        {
            CreateMap<ProductInfoResponse, ProductDTO>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ProductName))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.DiscountPrice,
                    opt => opt.MapFrom(src => src.Price - src.Price * src.Discount / 100))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Stock))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl));
        }
    }
}