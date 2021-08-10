using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FakeXiecheng.API.Dtos;
using FakeXiecheng.API.Moders;

namespace FakeXiecheng.API.Profiles
{
    public class TouristRoutePictureProfile:Profile
    {
        public TouristRoutePictureProfile()
        {
            CreateMap<TouristRoutePicture, TouristRoutePictureDto>();
            CreateMap<TouristRoutePictureForCreatetionDto, TouristRoutePicture>();
            CreateMap<TouristRoutePicture, TouristRoutePictureForCreatetionDto>();
        }
    }
}
