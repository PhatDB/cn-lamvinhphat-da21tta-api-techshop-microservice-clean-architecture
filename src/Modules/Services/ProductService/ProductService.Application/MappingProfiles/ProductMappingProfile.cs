using AutoMapper;
using ProductService.Application.DTOs;
using ProductService.Domain.Entities;

namespace ProductService.Application.Mappings
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<Product, GetAllProductDTO>()
                .ForMember(dest => dest.Sku, opt => opt.MapFrom(src => src.Sku.Value))
                .ForMember(dest => dest.FirstImageUrl,
                    opt => opt.MapFrom(src =>
                        src.ProductImages.FirstOrDefault().ImageUrl ?? string.Empty))
                .ForMember(dest => dest.StockQuantity,
                    opt => opt.MapFrom(src => src.Inventory.StockQuantity));

            CreateMap<Product, ProductDetailDTO>()
                .ForMember(dest => dest.Sku, opt => opt.MapFrom(src => src.Sku.Value))
                .ForMember(dest => dest.Images,
                    opt => opt.MapFrom(src =>
                        src.ProductImages.Select(img => img.ImageUrl).ToList()))
                .ForMember(dest => dest.StockQuantity,
                    opt => opt.MapFrom(src => src.Inventory.StockQuantity));
        }
    }
}