namespace WALKWAY_ECommerce.Models.Order_Model.OrderDto
{
    public class ViewOrderAdminDto
    {
        public int UserId { get; set; }
        public string orderId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string OrderStatus { get; set; }
        public DateTime OrderDate { get; set; }
        public string TransactionId { get; set; }
    }
}
