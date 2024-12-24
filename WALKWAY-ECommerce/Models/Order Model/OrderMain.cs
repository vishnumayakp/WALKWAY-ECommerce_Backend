using System.ComponentModel.DataAnnotations;
using WALKWAY_ECommerce.Models.Address_Model;
using WALKWAY_ECommerce.Models.User_Model;

namespace WALKWAY_ECommerce.Models.Order_Model
{
    public class OrderMain
    {
        public int OrderId { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        public int AddressId { get; set; }
        [Required]
        public decimal TotalAmount { get; set; }
        [Required]
        public string OrderStatus { get; set; }
        [Required]
        public string OrderString { get; set; }
        [Required]
        public string TransactionId { get; set; }

        public virtual User User { get; set; }
        public virtual Address Address { get; set; }
        public virtual List<OrderItem> OrderItems { get; set; }
    }
}
