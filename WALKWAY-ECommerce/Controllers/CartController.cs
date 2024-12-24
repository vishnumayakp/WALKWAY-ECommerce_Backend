using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WALKWAY_ECommerce.ApiResponse;
using WALKWAY_ECommerce.Models.Cart_Model.CartDto;
using WALKWAY_ECommerce.Services.Cart_Services;
using WALKWAY_ECommerce.Services.User_Service;

namespace WALKWAY_ECommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly IUserService _userService;

        public CartController(ICartService cartService, IUserService userService)
        {
            _cartService = cartService;
            _userService = userService;
        }

        [HttpGet("GetCartItems")]
        [Authorize(Roles ="user")]

        public async Task<IActionResult> GetCarts()
        {
            try
            {
                int userId = _userService.GetUserId();
                var cartitems = await _cartService.GetCartItems(userId);

                if (cartitems == null)
                {
                    return NotFound(new ApiResponses<string>(404, "Cart is Empty"));
                }

                return Ok(new ApiResponses<CartResDto>(200, "Cart Fetched Sucessfully", cartitems));
            } catch (Exception ex)
            {
                return StatusCode(500, new ApiResponses<string>(500, "Internal server Error", null, ex.Message));
            }
        }

        [HttpPost("AddToCart/{productId}")]
        [Authorize(Roles = "user")]

        public async Task<IActionResult> AddProductToCart(int productId)
        {
            try
            {
                int userId = _userService.GetUserId();
                var res = await _cartService.AddToCart(userId, productId);

                if (res.StatusCode == 200)
                {
                    return Ok(res);
                }
                else if (res.StatusCode == 404)
                {
                    return NotFound(res);
                }
                else if (res.StatusCode == 400)
                {
                    return BadRequest(res);
                }
                else if (res.StatusCode == 409)
                {
                    return Conflict(res);
                }
                else
                {
                    return StatusCode(500, res);
                }
            }catch(Exception ex)
            {
                return StatusCode(500, new ApiResponses<string>(500, "Internal server Error", null, ex.Message));
            }
        }

        [HttpDelete("Remove All CartItems")]
        [Authorize(Roles = "user")]

        public async Task<IActionResult> RemoveCartItems()
        {
            try
            {
                int userId = _userService.GetUserId();
                var res = await _cartService.RemoveAllItems(userId);

                if (!res)
                {
                    return BadRequest(new ApiResponses<string>(400, "Failed to Clear the Cart"));
                }
                return Ok(new ApiResponses<string>(200, "Successfully cleared Cart"));
            }catch(Exception ex)
            {
                return StatusCode(500, new ApiResponses<string>(500, "Internal server Error", null, ex.Message));
            }

        }

        [HttpDelete("{productId}")]
        [Authorize(Roles = "user")]

        public async Task<IActionResult> RemoveCartProduct(int productId)
        {
            try
            {
                int userId = _userService.GetUserId();

                var res = await _cartService.RemoveFromCart(userId, productId);
                if(!res)
                {
                    return BadRequest(new ApiResponses<string>(404, "Item Not Found in Cart"));
                }
                return Ok(new ApiResponses<string>(200, "Successfully Product Removed form Cart"));
            }catch(Exception ex)
            {
                return StatusCode(500, new ApiResponses<string>(500, "Internal server Error", null, ex.Message));
            }
        }

        [HttpPut("IncreaseQty/{productId}")]
        [Authorize(Roles ="user")]

        public async Task<IActionResult> IncreaseQuantity(int productId)
        {
            try
            {
                int userId = _userService.GetUserId();

                var res = await _cartService.IncreaseQty(userId, productId);

                if (res.StatusCode == 404)
                {
                    return NotFound(new ApiResponses<string>(404, "Not Found", null, res.Message));
                }else if(res.StatusCode == 400)
                {
                    return BadRequest(new ApiResponses<string>(400, "Bad Request", null, res.Message));
                }
                return Ok(res);
            }catch(Exception ex)
            {
                return StatusCode(500, new ApiResponses<string>(500, "Internal Server Error"));
            }
        }

        [HttpPut("DecreaseQty/{productId}")]
        [Authorize(Roles ="user")]

        public async Task<IActionResult> DecreaseQuantity(int productId)
        {
            try
            {
                int userId = _userService.GetUserId();
                var res = await _cartService.DecreaseQty(userId, productId);

                if (!res)
                {
                    return NotFound(new ApiResponses<string>(404, "Not Found ", null, "Item not found in Cart"));
                }

                return Ok(new ApiResponses<string>(200, "Item Decreased from Cart"));
            }catch(Exception ex)
            {
                return StatusCode(500, new ApiResponses<string>(500, "Internal Server Error", null, ex.Message));
            }
        }
    }
}
