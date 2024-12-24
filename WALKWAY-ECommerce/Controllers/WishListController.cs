using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WALKWAY_ECommerce.ApiResponse;
using WALKWAY_ECommerce.Models.WishList_Model.WishListDto;
using WALKWAY_ECommerce.Services.User_Service;
using WALKWAY_ECommerce.Services.WishList_Services;

namespace WALKWAY_ECommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishListController : ControllerBase
    {

        private readonly IWishListService _wishListService;
        private readonly IUserService _userService;
        private readonly ILogger<WishListController> _logger;

        public WishListController(IWishListService wishListService, IUserService userService, ILogger<WishListController> logger)
        {
            _wishListService = wishListService;
            _userService = userService;
            _logger = logger;
        }

        [HttpGet("WishList")]
        [Authorize]

        public async Task<IActionResult> GetWishLists()
        {
            try
            {
                int userId = _userService.GetUserId();
                var res = await _wishListService.GetWishLists(userId);

                if (res.Count == 0)
                {
                    return NotFound(new ApiResponses<List<WishListResponseDto>>(404,"Not Found",res,"Wish List is Empty"));
                }

                return Ok(new ApiResponses<List<WishListResponseDto>> (200, "wishList Fetched SuccessFully",res));

            }catch (Exception ex)
            {
                return StatusCode(500, new ApiResponses<string>(500, "Internal server Error", ex.Message));
            }

        }

        [HttpGet("AddOrReomve/{productId}")]
        public async Task<IActionResult> AddOrRemove(int productId)
        {
            try
            {
                _logger.LogInformation($"Attempting to add or remove item: ProductId = {productId}");
                int userId = _userService.GetUserId();

                var res = await _wishListService.AddOrRemove(userId, productId);

                if(res == "Product does not Exist.")
                {
                    return NotFound(new ApiResponses<string>(404, res));
                }

                return Ok(new ApiResponses<string>(200,res));
            }catch(Exception ex)
            {
                _logger.LogError($"Error occurred: {ex.Message}", ex);
                return StatusCode(500, new ApiResponses<string>(500, "Internal server Error", ex.Message));
            }
        }
    }
}
