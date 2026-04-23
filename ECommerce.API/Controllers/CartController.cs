using ECommerce.BLL;
using ECommerce.Common;
using ECommerce.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartManager _cartManager;

        public CartController(ICartManager cartManager)
        {
            _cartManager = cartManager;
        }
      
        [HttpGet]
        public async Task<ActionResult<GeneralResult<CartReadDto>>> GetUserCart()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var result = await _cartManager.GetUserCart(userId);
            return Ok(result);
        }
        [HttpPost]
        public async Task<ActionResult<GeneralResult<CartReadDto>>> AddToCart([FromBody] CartGetDto cartGetDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

        
            var result = await _cartManager.AddToCart(userId, cartGetDto);
            return Ok(result);
        }
        [HttpPut]
        public async Task<ActionResult<GeneralResult<CartReadDto>>> UpdateItemQuantity([FromBody] CartGetDto cartGetDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();


            var result = await _cartManager.UpdateCartItem(userId, cartGetDto);
            return Ok(result);
        }
        [HttpDelete]
        [Route("{productId:int}")]
        public async Task<ActionResult<GeneralResult<CartReadDto>>> RemoveFromCart(int productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();


            var result = await _cartManager.RemoveFromCart(userId, productId);
            return Ok(result);
        }

    }
}
