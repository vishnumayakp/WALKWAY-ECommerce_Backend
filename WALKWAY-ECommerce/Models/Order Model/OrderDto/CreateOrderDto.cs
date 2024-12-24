namespace WALKWAY_ECommerce.Models.Order_Model
{
    public class CreateOrderDto
    {
        public int AddressId { get; set; }
        public decimal Totalamount { get; set; }
        public string OrderString { get; set; }
        public string TransactionId { get; set; }

    }
}
