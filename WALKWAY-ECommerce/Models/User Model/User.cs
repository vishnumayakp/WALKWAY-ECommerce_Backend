using System.ComponentModel.DataAnnotations;
using WALKWAY_ECommerce.Models.Address_Model;
using WALKWAY_ECommerce.Models.Cart_Model;
using WALKWAY_ECommerce.Models.Order_Model;
using WALKWAY_ECommerce.Models.WishList_Model;

namespace WALKWAY_ECommerce.Models.User_Model
{
    public class User
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [MaxLength(20, ErrorMessage = "Name should not execeed 20 character")]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        public string? Password { get; set; }
        public string? Role { get; set; }
        public bool IsBlocked { get; set; }

        public List<WishList> WishLists { get; set; }
        public Cart Cart { get; set; }
        public virtual ICollection<Address> Addresses { get; set; }
        public virtual ICollection<OrderMain> Orders { get; set; }
    }
}
