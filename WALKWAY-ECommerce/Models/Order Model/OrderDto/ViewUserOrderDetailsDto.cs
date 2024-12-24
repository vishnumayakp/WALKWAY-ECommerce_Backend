using WALKWAY_ECommerce.Models.Cart_Model.CartDto;

namespace WALKWAY_ECommerce.Models.Order_Model.OrderDto
{
    public class ViewUserOrderDetailsDto
    {
        public DateTime OrderDate { get; set; }
        public string OrderId { get; set; }
        public string OrderStatus { get; set; }
        public string TransactionId { get; set; }
        public List<CartItemDto> OrderProducts { get; set; }
    }
}
