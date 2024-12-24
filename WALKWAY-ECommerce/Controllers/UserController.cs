using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WALKWAY_ECommerce.ApiResponse;
using WALKWAY_ECommerce.Models.User_Model.UserDto;
using WALKWAY_ECommerce.Services.AdminUser_Services;
using WALKWAY_ECommerce.Services.User_Service;

namespace WALKWAY_ECommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    { 
        private readonly IUserAdminService _userAdminService;

        public UserController(IUserAdminService userAdminService)
        {
            _userAdminService = userAdminService;
        }

        [HttpGet("AllUsers")]
        [Authorize(Roles="admin")]

        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var res = await _userAdminService.GetAllUsers();
                if(res == null)
                {
                    return NotFound(new ApiResponses<List<AdminUserResDto>>(404," Not Found",res,"Users Not Found"));
                }
                return Ok(new ApiResponses<List<AdminUserResDto>>(200, "Successfully Fetched All Users", res));
            }catch(Exception ex)
            {
                return StatusCode(500, new ApiResponses<string>(500, "Internal Server Error", "Data failed to upload"));
            }
        }

        [HttpGet("{userId}")]
        [Authorize(Roles ="admin")]

        public async Task<IActionResult> GetUserById(int userId)
        {
            try
            {
               var res= await _userAdminService.GetUserById(userId);
                if (res == null)
                {
                    return NotFound(new ApiResponses<AdminUserResDto>(404, " Not Found", res, $"User with id {userId} Not Found"));
                }
                return Ok(new ApiResponses<AdminUserResDto>(200, "Successfully fetched User", res));
                
            }catch(Exception ex)
            {
                return StatusCode(500, new ApiResponses<string>(500, "Internal Server Error", "Data failed to upload"));
            }
        }

        [HttpPatch("BlockOrUnblock{userId}")]
        [Authorize(Roles ="admin")]

        public async Task<IActionResult> BlockOrUnblockUser(int userId)
        {
            try
            {
                var res = await _userAdminService.BlockUnBlockUser(userId);
                return Ok(new ApiResponses<BlockUnblockRes>(200, "Block/Unblock Action Performed Successfully", res));
            }catch(Exception ex)
            {
                return StatusCode(500, new ApiResponses<string>(500, "Internal Server Error", "Data failed to upload"));
            }
        }
    }
}
