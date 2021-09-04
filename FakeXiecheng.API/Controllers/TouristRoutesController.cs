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
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace FakeXiecheng.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TouristRoutesController : ControllerBase
    {
        private ITouristRouteRepository _touristRouteRepository;
        private readonly IMapper _mapper;
        private readonly IUrlHelper _urlHelper;

        //通过构建函数，注入数据仓库的服务
        public TouristRoutesController(ITouristRouteRepository touristRouteRepository, IMapper mapper,
            IUrlHelperFactory urlHelperFactory,
            IActionContextAccessor actionContextAccessor
            )
        {
            _touristRouteRepository = touristRouteRepository;
            _mapper = mapper;
            _urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
        }

        private string GenerateTouristRouteResourceURL(
            [FromQuery] TouristRouteResourceParamaters paramaters,
            [FromQuery] PaginationResourceParamaters paramatertwo,
            ResourceUriType type)
        {
            return type switch
            {
                ResourceUriType.PreviousPage => _urlHelper.Link("GetTouristRoutes",
                    new
                    {
                        keyword = paramaters.Keyword,
                        rating = paramaters.Rating,
                        pageNumber = paramatertwo.PageNumber - 1,
                        pageSize = paramatertwo.PageSize
                    }),
                ResourceUriType.NextPage => _urlHelper.Link("GetTouristRoutes",
                    new
                    {
                        keyword = paramaters.Keyword,
                        rating = paramaters.Rating,
                        pageNumber = paramatertwo.PageNumber + 1,
                        pageSize = paramatertwo.PageSize
                    }),
                _ => _urlHelper.Link("GetTouristRoutes",
                    new
                    {
                        keyword = paramaters.Keyword,
                        rating = paramaters.Rating,
                        pageNumber = paramatertwo.PageNumber,
                        pageSize = paramatertwo.PageSize
                    })
            };
        }


        [HttpGet(Name = "GetTouristRoutes")]
        [HttpHead]
        //public async Task<IActionResult> GetTouristRoutes([FromQuery]int pagenumber,int pagesize)
        //{
        //     return Ok();
        //}
        public async Task<IActionResult> GetTouristRoutes([FromQuery] TouristRouteResourceParamaters paramaters,
                                                          [FromQuery] PaginationResourceParamaters paramatertwo)
        {

            var touristRoutesFromRepo = await _touristRouteRepository
                .GetTouristRoutesAsync(
                    paramaters.Keyword,
                    paramaters.RatingOperator,
                    paramaters.RatingValue,
                    paramatertwo.PageSize,
                    paramatertwo.PageNumber
                );
            if (touristRoutesFromRepo == null || touristRoutesFromRepo.Count() <= 0)
            {
                return NotFound("没有旅游路线");
            }
            var touristRouteDto = _mapper.Map<IEnumerable<TouristRouteDto>>(touristRoutesFromRepo);
            var previousPageLink = touristRoutesFromRepo.HasPrevious 
                ? GenerateTouristRouteResourceURL(paramaters, paramatertwo, ResourceUriType.PreviousPage) 
                : null;
            var nextPageLink = touristRoutesFromRepo.HasNext
                ? GenerateTouristRouteResourceURL(paramaters, paramatertwo, ResourceUriType.NextPage)
                : null;

            var paginationMetadata = new
            {
                previousPageLink,
                nextPageLink,
                totalCount = touristRoutesFromRepo.TotalCount,
                pageSize = touristRoutesFromRepo.PageSize,
                currentPage = touristRoutesFromRepo.CurrentPage,
                totalPages = touristRoutesFromRepo.TotalPages
            };

            Response.Headers.Add("x-pagination", Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

            return Ok(touristRouteDto);
        }

        [HttpGet("{touristRouteId}", Name = "GetTouristRouteById")]
        public async Task<IActionResult> GetTouristRouteById(Guid touristRouteId)
        {
            var touristRouteFromRepo = await _touristRouteRepository.GetTouristRouteAsync(touristRouteId);
            if (touristRouteFromRepo == null)
            {
                return NotFound($"路由路线{touristRouteId}找不到");
            }
            var touristRouteDto = _mapper.Map<TouristRouteDto>(touristRouteFromRepo);
            return Ok(touristRouteDto);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> CrateTouristRoute([FromBody] TouristRouteForCreationDto touristRouteForCreationDto)
        {
            var touristRouteModel = _mapper.Map<TouristRoute>(touristRouteForCreationDto);
            _touristRouteRepository.AddTouristRoute(touristRouteModel);
            await _touristRouteRepository.SaveAsync();
            var touristRouteToReture = _mapper.Map<TouristRouteDto>(touristRouteModel);
            return CreatedAtRoute(
                "GetTouristRouteById",
                new { touristRouteId = touristRouteToReture.Id },
                touristRouteToReture
                );

        }

       [HttpPut("{touristRouteId}")]
        public async Task<IActionResult> UpdateTouristRoute([FromRoute]Guid touristRouteId, [FromBody]TourisRouteForUpdateDto tourisRouteForUpdateDto)
        {
            if (!(await _touristRouteRepository.TrouristRouteExistsAsync(touristRouteId)))
            {
                return NotFound("旅游路线不存在");
            }
            var touristRouteFromRepo = await _touristRouteRepository.GetTouristRouteAsync(touristRouteId);
            //1；映射DTO
            //更新dto
            //映射model
            _mapper.Map(tourisRouteForUpdateDto, touristRouteFromRepo);
            await _touristRouteRepository.SaveAsync();

            return NoContent();
        }

        [HttpPatch("{touristRouteId}")]
        public async Task<IActionResult> PartiallyUpdateToutistRoute([FromRoute]Guid touristRouteId,[FromBody]JsonPatchDocument<TourisRouteForUpdateDto> patchDocument)
        {
            if (!(await _touristRouteRepository.TrouristRouteExistsAsync(touristRouteId)))
            {
                return NotFound("旅游路线不存在");
            }

            var touristRouteFromRepo = await _touristRouteRepository.GetTouristRouteAsync(touristRouteId);

            var touristRouteToPatch = _mapper.Map<TourisRouteForUpdateDto>(touristRouteFromRepo);

            patchDocument.ApplyTo(touristRouteToPatch, ModelState);
            if (!TryValidateModel(touristRouteToPatch))
            {
                return ValidationProblem(ModelState);
            };
            _mapper.Map(touristRouteToPatch, touristRouteFromRepo);
            await _touristRouteRepository.SaveAsync();

            return NoContent();
        }

        public async Task<IActionResult> DeleteTourisRoute([FromRoute] Guid touristRouteId) 
        {
            if (!(await _touristRouteRepository.TrouristRouteExistsAsync(touristRouteId)))
            {
                return NotFound("旅游路线不存在");
            }
            var touristRoute =await _touristRouteRepository.GetTouristRouteAsync(touristRouteId);
            _touristRouteRepository.DeleteTouristRoute(touristRoute);
            await _touristRouteRepository.SaveAsync();
            return NoContent();
        }

        [HttpDelete("{touristIDs}")]
        public async Task<IActionResult> DeleteByIDs([ModelBinder(BinderType =typeof(ArrayModelBinder))][FromRoute]IEnumerable<Guid> touristIDs)
        {
            if (touristIDs == null)
            {
                return BadRequest();
            }
            var touristRoutesFromRepo = await _touristRouteRepository.GetTouritRoutesByIDListAsync(touristIDs);
            _touristRouteRepository.DeleteTouristRoutes(touristRoutesFromRepo);
            await _touristRouteRepository.SaveAsync();

            return NoContent();

        }
    }
}
