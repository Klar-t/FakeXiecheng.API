﻿using AutoMapper;
using FakeXiecheng.API.Dtos;
using FakeXiecheng.API.Helper;
using FakeXiecheng.API.Moders;
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
    [Route("api/shoppingCart")]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITouristRouteRepository _touristRouteRepository;
        private readonly IMapper _mapper;

        public ShoppingCartController(IHttpContextAccessor httpContextAccessor,
            ITouristRouteRepository touristRouteRepository
            , IMapper mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _touristRouteRepository = touristRouteRepository;
            _mapper = mapper;
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetShoppingCart()
        {

            //获得当前用户
            var userId = _httpContextAccessor
                .HttpContext.User
                .FindFirst(ClaimTypes.NameIdentifier).Value;

            //使用userid获得购物车
            var shoppingCart = await _touristRouteRepository.GetShoppingCarByUserIdAsync(userId);


            return Ok(_mapper.Map<ShoppingCartDto>(shoppingCart));


        }

        [HttpPost("items")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> AddShoppingCartItem([FromBody] AddShoppingCartItemDto addShoppingCartItemDto)
        {
            //获得当前用户
            var userId = _httpContextAccessor
                .HttpContext.User
                .FindFirst(ClaimTypes.NameIdentifier).Value;

            //使用userid获得购物车
            var shoppingCart = await _touristRouteRepository.GetShoppingCarByUserIdAsync(userId);


            //创建lineItem
            var touristRoute =await _touristRouteRepository
                .GetTouristRouteAsync(addShoppingCartItemDto.TouristRouteId);
            if (touristRoute == null)
            {
                return NotFound("旅游路线不存在");
            }

            var lineItem = new LineItem()
            {
                TouristRouteId = addShoppingCartItemDto.TouristRouteId,
                ShoppingCartId = shoppingCart.Id,
                OriginalPrice = touristRoute.OriginalPrice,
                DiscountPresent = touristRoute.DiscountPresent
            };

            //添加lineItem,并保存数据库
            await _touristRouteRepository.AddShoppingCartItem(lineItem);
            await _touristRouteRepository.SaveAsync();

            return Ok(_mapper.Map<ShoppingCartDto>(shoppingCart)); 
        }
        [HttpDelete("items/{itemId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> DeleteShoppingCartItem([FromRoute] int itemId)
        {
            //1、获取lineitem数据
            var lineItem = await _touristRouteRepository.GetShoppingCartItemByItemIdAsync(itemId);
            if (lineItem == null)
            {
                return NotFound("购物车商品找不到");
            }
            _touristRouteRepository.DeleteShoppingCartItem(lineItem);
            await _touristRouteRepository.SaveAsync();

            return NoContent();
        }

        [HttpDelete("items/({itemIds})")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> RemoveShoppingCartItems(
            [ModelBinder(BinderType =typeof(ArrayModelBinder))]
            [FromRoute] IEnumerable<int> itemIds)
        {
            var lineitems = await _touristRouteRepository
                .GetShoppingCartByIdListAsync(itemIds);

            _touristRouteRepository.DeleteShoppingCartItems(lineitems);
            await _touristRouteRepository.SaveAsync();

            return NoContent();
        }



    } 
}
