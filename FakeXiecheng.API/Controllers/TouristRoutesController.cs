using FakeXiecheng.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FakeXiecheng.API.Dtos;
using System.Text.RegularExpressions;
using FakeXiecheng.API.Moders;
using Microsoft.AspNetCore.Authorization;
using FakeXiecheng.API.ResourceParameters;
using Microsoft.AspNetCore.JsonPatch;
using FakeXiecheng.API.Helper;

namespace FakeXiecheng.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TouristRoutesController : ControllerBase
    {
        private ITouristRouteRepository _touristRouteRepository;
        private readonly IMapper _mapper;

        //通过构建函数，注入数据仓库的服务
        public TouristRoutesController(ITouristRouteRepository touristRouteRepository, IMapper mapper)
        {
            _touristRouteRepository = touristRouteRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetTouristRoutes([FromQuery] TouristRouteResourceParamaters paramaters)
        {
            var touristRoutesFromRepo = _touristRouteRepository.GetTouristRoutes(paramaters);
            if (touristRoutesFromRepo == null || touristRoutesFromRepo.Count() <= 0)
            {
                return NotFound("没有旅游路线");
            }
            var touristRouteDto = _mapper.Map<IEnumerable<TouristRouteDto>>(touristRoutesFromRepo);
            return Ok(touristRouteDto);
        }

        [HttpGet("{touristRouteId}", Name = "GetTouristRouteById")]
        public IActionResult GetTouristRouteById(Guid touristRouteId)
        {
            var touristRouteFromRepo = _touristRouteRepository.GetTouristRoute(touristRouteId);
            if (touristRouteFromRepo == null)
            {
                return NotFound($"路由路线{touristRouteId}找不到");
            }
            var touristRouteDto = _mapper.Map<TouristRouteDto>(touristRouteFromRepo);
            return Ok(touristRouteDto);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public IActionResult CrateTouristRoute([FromBody] TouristRouteForCreationDto touristRouteForCreationDto)
        {
            var touristRouteModel = _mapper.Map<TouristRoute>(touristRouteForCreationDto);
            _touristRouteRepository.AddTouristRoute(touristRouteModel);
            _touristRouteRepository.Save();
            var touristRouteToReture = _mapper.Map<TouristRouteDto>(touristRouteModel);
            return CreatedAtRoute(
                "GetTouristRouteById",
                new { touristRouteId = touristRouteToReture.Id },
                touristRouteToReture
                );

        }

       [HttpPut("{touristRouteId}")]
        public IActionResult UpdateTouristRoute([FromRoute]Guid touristRouteId, [FromBody]TourisRouteForUpdateDto tourisRouteForUpdateDto)
        {
            if (!_touristRouteRepository.TrouristRouteExists(touristRouteId))
            {
                return NotFound("旅游路线不存在");
            }
            var touristRouteFromRepo = _touristRouteRepository.GetTouristRoute(touristRouteId);
            //1；映射DTO
            //更新dto
            //映射model
            _mapper.Map(tourisRouteForUpdateDto, touristRouteFromRepo);
            _touristRouteRepository.Save();

            return NoContent();
        }

        [HttpPatch("{touristRouteId}")]
        public IActionResult PartiallyUpdateToutistRoute([FromRoute]Guid touristRouteId,[FromBody]JsonPatchDocument<TourisRouteForUpdateDto> patchDocument)
        {
            if (!_touristRouteRepository.TrouristRouteExists(touristRouteId))
            {
                return NotFound("旅游路线不存在");
            }

            var touristRouteFromRepo = _touristRouteRepository.GetTouristRoute(touristRouteId);

            var touristRouteToPatch = _mapper.Map<TourisRouteForUpdateDto>(touristRouteFromRepo);

            patchDocument.ApplyTo(touristRouteToPatch, ModelState);
            if (!TryValidateModel(touristRouteToPatch))
            {
                return ValidationProblem(ModelState);
            };
            _mapper.Map(touristRouteToPatch, touristRouteFromRepo);
            _touristRouteRepository.Save();

            return NoContent();
        }

        public IActionResult DeleteTourisRoute([FromRoute] Guid touristRouteId) 
        {
            if (!_touristRouteRepository.TrouristRouteExists(touristRouteId))
            {
                return NotFound("旅游路线不存在");
            }
            var touristRoute = _touristRouteRepository.GetTouristRoute(touristRouteId);
            _touristRouteRepository.DeleteTouristRoute(touristRoute);
            _touristRouteRepository.Save();
            return NoContent();
        }

        [HttpDelete("{touristIDs}")]
        public IActionResult DeleteByIDs([ModelBinder(BinderType =typeof(ArrayModelBinder))][FromRoute]IEnumerable<Guid> touristIDs)
        {
            if (touristIDs == null)
            {
                return BadRequest();
            }
            var touristRoutesFromRepo = _touristRouteRepository.GetTouritRoutesByIDList(touristIDs);
            _touristRouteRepository.DeleteTouristRoutes(touristRoutesFromRepo);
            _touristRouteRepository.Save();

            return NoContent();

        }
    }
}
