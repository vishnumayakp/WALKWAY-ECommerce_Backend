using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WALKWAY_ECommerce.ApiResponse;
using WALKWAY_ECommerce.Models.User_Model.UserDTO;
using WALKWAY_ECommerce.Services.AuthService;

namespace WALKWAY_ECommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("Register")]

        public async Task<IActionResult> RegisterUser([FromBody] UserRegistrationDto newUser)
        {
            try
            {
                if(newUser == null)
                {
                    return BadRequest(new ApiResponses<string>(400, "User cannot be null"));
                }
                bool isDone = await _authService.Register(newUser);

                if (!isDone)
                {
                    return Conflict(new ApiResponses<string>(409, "User already Exist"));
                }
                return Ok(new ApiResponses<bool>(200, "User Successfully Registered", isDone));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponses<string>(500, "an error Occured", null, ex.Message));
            }
        }

        [HttpPost("Login")]

        public async Task<IActionResult> LoginUser([FromBody] UserLoginDto logUser)
        {
            try
            {
                var res = await _authService.Login(logUser);
                _logger.LogInformation($"result is {res}");

                if (res == null || !string.IsNullOrEmpty(res.Error))
                {

                    if (res?.Error == "User not Found")
                    {
                        return StatusCode(404,new ApiResponses<string>(404,"Please SignUp,User not found"));
                    }

                    if (res.Error== "User is blocked by admin")
                    {
                        return StatusCode(403, new ApiResponses<string>(403, "User Account Is Blocked"));
                    }

                    if (res?.Error == "Invalid password")
                    {
                        return BadRequest(new ApiResponses<string>(400, "BadRequest", null, res.Error));
                    }
                    //return BadRequest(new { error = "Invalid Credentials" });
                }
                return Ok(new ApiResponses<UserResDto>(200,"Login Successful",res));
            }catch (Exception ex)
            {
                return StatusCode(500,new ApiResponses<string>(500, "Internal Server Error", null,ex.Message));
            }
        
        }
    }
}
