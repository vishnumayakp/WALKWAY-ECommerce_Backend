namespace WALKWAY_ECommerce.Models.User_Model.UserDTO
{
    public class UserResDto
    {
        //public int Id { get; set; }
        public string Token { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        //public DateTime TokenExpiration { get; set; }
        public bool IsBlocked { get; set; }
        public string Error { get; set; }
    }
}
