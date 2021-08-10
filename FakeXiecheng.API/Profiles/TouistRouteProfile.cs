using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FakeXiecheng.API.Moders;
using FakeXiecheng.API.Dtos;

namespace FakeXiecheng.API.Profiles
{
    public class TouistRouteProfile:Profile
    {
        public TouistRouteProfile()
        {
            CreateMap<TouristRoute, TouristRouteDto>()
                .ForMember(
                    dest => dest.Price,
                    opt => opt.MapFrom(src => src.OriginalPrice * (decimal)(src.DiscountPresent ?? 1))
                 )
                .ForMember(
                    dest => dest.travelDays,
                    opt => opt.MapFrom(src => src.travelDays.ToString()
                 ))
                 .ForMember(
                    dest => dest.TripType,
                    opt => opt.MapFrom(src => src.TripType.ToString()
                 ))
                .ForMember(
                    dest => dest.DepartureCity,
                    opt => opt.MapFrom(src => src.DepartureCity.ToString()
                 ));

            CreateMap<TouristRouteForCreationDto, TouristRoute>()
                .ForMember(
                    dest => dest.Id,
                    opt => opt.MapFrom(src => Guid.NewGuid())
                );

            CreateMap<TourisRouteForUpdateDto, TouristRoute>();

            CreateMap<TouristRoute, TourisRouteForUpdateDto>();
        }
    }
}
