using AutoMapper;
using FakeXiecheng.API.Dtos;
using FakeXiecheng.API.Moders;
using FakeXiecheng.API.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FakeXiecheng.API.Controllers
{
    [Route("api/TouristRoutes/{touristrouteId}/picture")]
    [ApiController]
    public class TouristRoutePicturesController:ControllerBase
    {
        private ITouristRouteRepository _touristRouteRepository;
        private IMapper _mapper;

        public TouristRoutePicturesController(ITouristRouteRepository touristRouteRepository, IMapper mapper)
        {
            _touristRouteRepository = touristRouteRepository??
                throw new ArgumentNullException(nameof(touristRouteRepository));
            _mapper = mapper??
                throw new ArgumentNullException(nameof(Mapper));
        }

        public async Task<IActionResult> GetPictureListForTouristRoute(Guid touristrouteId)
        {
            if (!(await _touristRouteRepository.TrouristRouteExistsAsync(touristrouteId)))
            {
                return NotFound("旅游路线不存在！");
            }

            var PictureForRepo = await _touristRouteRepository.GetPictureForTouristRouteAsync(touristrouteId);
            if (PictureForRepo == null || PictureForRepo.Count() <= 0)
            {
                return NotFound("旅游路线照片不存在");
            }
            var touristRoutePiceDto = _mapper.Map<IEnumerable<TouristRoutePictureDto>>(PictureForRepo);
            return Ok(touristRoutePiceDto);
        }

        [HttpGet("{pictureId}", Name = "GetPicture")]
        public async Task<IActionResult> GetPicture(Guid touristrouteId, int pictureId)
        {
            if (!(await _touristRouteRepository.TrouristRouteExistsAsync(touristrouteId)))
            {
                return NotFound("旅游路线不存在！");
            }
            var picetureRepo =await _touristRouteRepository.GetPictureAsync(pictureId);
            if (picetureRepo==null)
            {
                return NotFound("旅游照片不存在！");
            }
            return Ok(_mapper.Map<TouristRoutePictureDto>(picetureRepo));
        }


        [HttpPost]
        public async Task<IActionResult> CreateTouristRoutePicture([FromRoute]Guid touristRouteId,
            [FromBody] TouristRoutePictureForCreatetionDto touristRoutePictureForCreatetionDto)
        {
            if (!(await _touristRouteRepository.TrouristRouteExistsAsync(touristRouteId)))
            {
                return NotFound("旅游路线不存在！");
            }
            var pictureModel = _mapper.Map<TouristRoutePicture>(touristRoutePictureForCreatetionDto);
            _touristRouteRepository.AddTouristRoutePicture(touristRouteId, pictureModel);
            await _touristRouteRepository.SaveAsync();

            var pictureToReturn = _mapper.Map<TourisRouteForUpdateDto>(pictureModel);
            return CreatedAtRoute(
                "GetPicture",
                new
                {
                    touristRouteId = pictureModel.TouristRouteId,
                    pictureId = pictureModel.Id
                },
                pictureToReturn
                );

        }


        [HttpDelete("{pictureId}")]
        public async Task<IActionResult> DeletePictureAsync([FromRoute]Guid touristrouteId,[FromRoute]int pictureid)
        {
            if (!await _touristRouteRepository.TrouristRouteExistsAsync(touristrouteId))
            {
                return NotFound("旅游路线不存在！");
            }
            var picture = await _touristRouteRepository.GetPictureAsync(pictureid);
            _touristRouteRepository.DeleteTouristRoutePicture(picture);
            await _touristRouteRepository.SaveAsync();

            return NoContent();
        }

    }
}
