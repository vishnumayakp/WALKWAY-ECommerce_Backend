using WALKWAY_ECommerce.Models.User_Model.UserDto;

namespace WALKWAY_ECommerce.Services.AdminUser_Services
{
    public interface IUserAdminService
    {
        Task<List<AdminUserResDto>> GetAllUsers();
        Task<AdminUserResDto> GetUserById(int userId);
        Task<BlockUnblockRes> BlockUnBlockUser(int userId);
    }
}
