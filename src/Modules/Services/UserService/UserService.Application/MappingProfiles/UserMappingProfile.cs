using AutoMapper;
using UserService.Application.DTOs;
using UserService.Domain.Entities;

namespace UserService.Application.MappingProfiles
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, UserDTO>().ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id)).ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Value))
                .ForMember(dest => dest.PhoneNumber,
                    opt => opt.MapFrom(src => src.UserAddresses.FirstOrDefault() != null ? src.UserAddresses.FirstOrDefault().PhoneNumber.Value : string.Empty))
                .ForMember(dest => dest.AddressLine,
                    opt => opt.MapFrom(src => src.UserAddresses.FirstOrDefault() != null ? src.UserAddresses.FirstOrDefault().AddressLine : string.Empty))
                .ForMember(dest => dest.Province,
                    opt => opt.MapFrom(src => src.UserAddresses.FirstOrDefault() != null ? src.UserAddresses.FirstOrDefault().Province : string.Empty)).ForMember(
                    dest => dest.District, opt => opt.MapFrom(src => src.UserAddresses.FirstOrDefault() != null ? src.UserAddresses.FirstOrDefault().District : string.Empty));
        }
    }
}