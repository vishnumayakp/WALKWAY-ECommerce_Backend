namespace WALKWAY_ECommerce.Models.Cart_Model.CartDto
{
    public class CartItemDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public string Image { get; set; }

    }
}
