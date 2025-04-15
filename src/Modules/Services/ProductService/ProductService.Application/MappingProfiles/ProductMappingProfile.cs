using AutoMapper;
using ProductService.Application.DTOs;
using ProductService.Domain.Entities;

namespace ProductService.Application.MappingProfiles
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<Product, GetAllProductDTO>().ForMember(dest => dest.ImageUrl,
                opt => opt.MapFrom(src =>
                    src.ProductImages.FirstOrDefault(i => i.IsMain == true)!.ImageUrl ?? string.Empty));

            CreateMap<Product, ProductDetailDTO>()
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.ProductImages))
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Id));


            CreateMap<ProductImage, ImageDto>().ForMember(dest => dest.ImageId, opt => opt.MapFrom(src => src.Id));
        }
    }
}