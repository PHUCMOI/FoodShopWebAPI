using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.ModelRequest;
using ModelLayer.ModelResponse;
using ModelLayer.Models;
using ServiceLayer.ServiceInterfaces;
using Services_Layer.Service;
using Services_Layer.ServiceInterfaces;

namespace FoodShopWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartService cartService;
        public CartController(ICartService cartService) 
        {
            this.cartService = cartService;
        }
        // GET: CartController
        [HttpGet]
        public async Task<ActionResult<CartResponse>> GetListCartbyUserAsync(int userId)
        {
            try
            {
                var cartList = await cartService.GetCarts(userId);
                if (cartList != null)
                {
                    return Ok(cartList);
                }
                else
                {
                    return BadRequest("category list is null");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("AddToCart")]
        public async Task<ActionResult> AddToCart([FromBody] CartRequest cart)
        {
            try
            {
                if (cart != null)
                {
                    var result = await cartService.CreateProductCart(cart);
                    if (result)
                    {
                        return Ok(result);
                    }
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("UpdateCart")]
        public async Task<ActionResult> UpdateCart([FromBody] CartRequest cart)
        {
            try
            {
                if (cart != null)
                {
                    var result = await cartService.UpdateProductCart(cart);
                    if (result)
                    {
                        return Ok(result);
                    }
                }
                return BadRequest();
            }
            catch (Exception ex)    
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("DeleteItem")]
        public async Task<ActionResult> DeleteItem([FromBody] CartRequest cart)
        {
            try
            {
                if (cart != null)
                {
                    var result = await cartService.DeleteItem(cart);
                    if (result)
                    {
                        return Ok(result);
                    }
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("clearCart")]
        public async Task<IActionResult> ClearCart([FromBody] int id)
        {
            try
            {
                if (id > 0)
                {
                    var result = await cartService.ClearCart(id);
                    if (result)
                    {
                        return Ok(result);  
                    }
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
