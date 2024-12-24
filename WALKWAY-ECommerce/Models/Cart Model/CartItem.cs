using WALKWAY_ECommerce.Models.Product_Model;

namespace WALKWAY_ECommerce.Models.Cart_Model
{
    public class CartItem
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public Cart Cart { get; set; }
        public Product Product { get; set; }
    }
}
