namespace WALKWAY_ECommerce.Models.Order_Model.OrderDto
{
    public class PaymentDto
    {
        public string? razorpay_payment_id { get; set; }
        public string? razorpay_orderId { get; set; }
        public string? razorpay_signature { get; set; }
    }
}
