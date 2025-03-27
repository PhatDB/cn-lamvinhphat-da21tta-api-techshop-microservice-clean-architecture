using AutoMapper;
using UserService.Application.DTOs;
using UserService.Domain.Entities;

namespace UserService.Application.MappingProfiles
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, UserDTO>().ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id)).ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Value))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.UserAddresses.FirstOrDefault().PhoneNumber.Value))
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.UserAddresses.FirstOrDefault().Street))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.UserAddresses.FirstOrDefault().City))
                .ForMember(dest => dest.District, opt => opt.MapFrom(src => src.UserAddresses.FirstOrDefault().District))
                .ForMember(dest => dest.Ward, opt => opt.MapFrom(src => src.UserAddresses.FirstOrDefault().Ward))
                .ForMember(dest => dest.ZipCode, opt => opt.MapFrom(src => src.UserAddresses.FirstOrDefault().ZipCode));
        }
    }
}