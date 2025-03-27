using AutoMapper;
using BuildingBlocks.Contracts.Products;
using CartService.Application.DTOs;

namespace CartService.Application.MappingProfiles
{
    public class CartMappingProfile : Profile
    {
        public CartMappingProfile()
        {
            CreateMap<ProductInfoResponse, ProductDTO>().ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name)).ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.StockQuantity)).ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl));
        }
    }
}