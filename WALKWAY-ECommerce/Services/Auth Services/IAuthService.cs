using WALKWAY_ECommerce.Models.User_Model.UserDTO;

namespace WALKWAY_ECommerce.Services.AuthService
{

    public interface IAuthService
    {
        Task<bool> Register(UserRegistrationDto userRegistrationDto);
        Task<UserResDto> Login(UserLoginDto userLoginDto);
    }
}
