namespace WALKWAY_ECommerce.Models.User_Model.UserDto
{
    public class AdminUserResDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public bool IsBlocked { get; set; }
    }
}
