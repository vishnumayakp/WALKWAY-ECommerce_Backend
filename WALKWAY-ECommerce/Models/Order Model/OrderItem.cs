using WALKWAY_ECommerce.Models.Product_Model;

namespace WALKWAY_ECommerce.Models.Order_Model
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        public int productId { get; set; }
        public decimal TotalPrice { get; set; }
        public int Quantity { get; set; }

        public virtual Product Product { get; set; }
        public virtual OrderMain Order { get; set; }

    }
}
