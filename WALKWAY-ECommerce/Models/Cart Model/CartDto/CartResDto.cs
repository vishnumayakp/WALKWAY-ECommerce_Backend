namespace WALKWAY_ECommerce.Models.Cart_Model.CartDto
{
    public class CartResDto
    {
        public List<CartItemDto> CartItems { get; set; }
        public int TotalItems { get; set; }
        public decimal GrandTotal { get; set; }
      
    }
}
