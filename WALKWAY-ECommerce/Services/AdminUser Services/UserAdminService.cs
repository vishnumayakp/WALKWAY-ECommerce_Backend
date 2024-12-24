using Microsoft.EntityFrameworkCore;
using WALKWAY_ECommerce.DbContexts;
using WALKWAY_ECommerce.Models.User_Model.UserDto;

namespace WALKWAY_ECommerce.Services.AdminUser_Services
{
    public class UserAdminService:IUserAdminService
    {
        private readonly AppDbContext _appDbContext;

        public UserAdminService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<AdminUserResDto>> GetAllUsers()
        {
            try
            {
                var users = await _appDbContext.Users.Where(u=>u.Role!="admin").Select(user => new AdminUserResDto
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Role = user.Role,
                    IsBlocked = user.IsBlocked,
                }).ToListAsync();
                return users;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<AdminUserResDto> GetUserById(int userId)
        {
            try
            {
                var user = await _appDbContext.Users.Where(u => u.Role != "admin").FirstOrDefaultAsync(u => u.Id == userId);

                if(user == null)
                {
                    return null;
                }
                return new AdminUserResDto
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Role = user.Role,
                    IsBlocked = user.IsBlocked
                };
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BlockUnblockRes> BlockUnBlockUser(int userId)
        {
            try
            {
                var user = await _appDbContext.Users.FindAsync(userId);

                if (user == null)
                {
                    throw new Exception("User Not Found");
                }

                user.IsBlocked = !user.IsBlocked;
                await _appDbContext.SaveChangesAsync();

                return new BlockUnblockRes
                {
                    IsBlocked = user.IsBlocked == true ? true : false,
                    Message = user.IsBlocked == true ? "User Is Blocked" : "User Is Unblocked"
                };
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
