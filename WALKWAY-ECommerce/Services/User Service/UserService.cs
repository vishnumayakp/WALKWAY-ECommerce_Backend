using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WALKWAY_ECommerce.DbContexts;
using WALKWAY_ECommerce.Models.User_Model.UserDto;
using WALKWAY_ECommerce.Models.User_Model.UserDTO;

namespace WALKWAY_ECommerce.Services.User_Service
{
    public class UserService:IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int GetUserId()
        {
            var UserIdClaim=_httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(UserIdClaim != null && int.TryParse(UserIdClaim,out int userId))
            {
                return userId;
            }

            throw new Exception("User is Not Authenticated");
        }
    }
}
