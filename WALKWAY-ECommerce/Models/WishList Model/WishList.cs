using WALKWAY_ECommerce.Models.Product_Model;
using WALKWAY_ECommerce.Models.User_Model;

namespace WALKWAY_ECommerce.Models.WishList_Model
{
    public class WishList
    {
        public int WishListId { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }

        public User User { get; set; }
        public Product Product { get; set; }
    }
}
