using AutoMapper;
using FakeXiecheng.API.Dtos;
using FakeXiecheng.API.ResourceParameters;
using FakeXiecheng.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FakeXiecheng.API.Controllers
{
    [ApiController]
    [Route("apo/orders")]
    public class OrdersController:ControllerBase
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITouristRouteRepository _touristRouteRepository;
        private readonly IMapper _mapper;

        public OrdersController(IHttpContextAccessor httpContextAccessor, ITouristRouteRepository touristRouteRepository,IMapper mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _touristRouteRepository = touristRouteRepository;
            _mapper = mapper;
        }


        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetOrders([FromQuery] PaginationResourceParamaters paramatertwo)
        {
            //获取当前用户
            var userId = _httpContextAccessor
                .HttpContext.User
                .FindFirst(ClaimTypes.NameIdentifier).Value;

            //使用用户id来获取订单历史记录
            var orders = await _touristRouteRepository.GetOrderByUserId(userId,paramatertwo.PageSize,paramatertwo.PageNumber);

            return Ok(_mapper.Map<IEnumerable<OrderDto>>(orders));
        }

        [HttpGet("{orderId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetOrderById([FromRoute] Guid orderId)
        {
            //获取当前用户
            var userId = _httpContextAccessor
                .HttpContext.User
                .FindFirst(ClaimTypes.NameIdentifier).Value;

            var order = await _touristRouteRepository.GetOrderById(orderId);

            return Ok(_mapper.Map<OrderDto>(order));

        }
    }
}
