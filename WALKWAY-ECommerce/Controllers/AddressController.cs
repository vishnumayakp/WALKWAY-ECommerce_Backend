using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WALKWAY_ECommerce.ApiResponse;
using WALKWAY_ECommerce.Models.Address_Model.AddressDto;
using WALKWAY_ECommerce.Services.Address_Services;
using WALKWAY_ECommerce.Services.User_Service;

namespace WALKWAY_ECommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;
        private readonly IUserService _userService;

        public AddressController(IAddressService addressService, IUserService userService)
        {
            _addressService = addressService;
            _userService = userService;
        }

        [HttpPost("Add Address")]
        [Authorize(Roles = "user")]

        public async Task<IActionResult> AddAddress([FromBody] AddressDto addressDto)
        {
            try
            {
                int userId = _userService.GetUserId();
                await _addressService.CreateNewAddress(userId, addressDto);

                return Ok(new ApiResponses<string>(200, "Successfully Added Address"));
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new ApiResponses<string>(400, ex.Message));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, new ApiResponses<string>(500, "Internal Server Error", "Data failed to upload"));
            }
        }

        [HttpGet("getAddress")]

        public async Task<IActionResult> GetAddressById()
        {
            try
            {
                var userId = _userService.GetUserId();
                var res = await _addressService.GetAddressById(userId);

                if (res == null || res.Count == 0)
                {
                    return NotFound(new ApiResponses<List<AddressResDto>>(404, "Not found", res, "Address Not Found for User"));
                }

                return Ok(new ApiResponses<List<AddressResDto>>(200, "User Address Fetched Successfully", res));
            } catch (Exception ex)
            {
                return StatusCode(500, new ApiResponses<string>(500, "Internal Server Error", "Data failed to Fetch"));
            }
        }

        [HttpPut("Update Address{userId}")]

        public async Task<IActionResult> UpdateAddressbyId([FromBody] UpdateAddressDto updateAddressDto)
        {
            try
            {
                int userId = _userService.GetUserId();
                var res = await _addressService.UpdateAddress(userId, updateAddressDto);

                if (!res)
                {
                    return BadRequest(new ApiResponses<string>(400, "Failed to Update Address"));
                }

                return Ok(new ApiResponses<string>(200, "Successfully updated Address"));
            } catch (Exception ex)
            {
                return StatusCode(500, new ApiResponses<string>(500, "Internal Server Error", ex.Message));
            }
        }

        [HttpDelete("{addressId}")]
        //[Authorize(Roles ="admin")]
        public async Task<IActionResult> RemoveAddress(int addressId)
        {
            try
            {
                int userId = _userService.GetUserId();

                var result = await _addressService.DeleteAddress(addressId, userId);

                if (!result)
                {
                    return NotFound(new ApiResponses<string>(404, "Not Found", null, $"Address with userId {userId} not found"));
                }
                return Ok(new ApiResponses<string>(200, $"Successfully removed address with userId {userId}"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponses<string>(500, "Internal Server Error", ex.Message));
            }
        }




    }
}
