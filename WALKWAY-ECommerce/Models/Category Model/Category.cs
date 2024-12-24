using System.ComponentModel.DataAnnotations;
using WALKWAY_ECommerce.Models.Product_Model;

namespace WALKWAY_ECommerce.Models.Cart_Model
{
    public class Category
    {
        public int CategoryId { get; set; }

        [Required]
        public string? CategoryName { get; set; }

        [Required]
        public ICollection<Product> Products { get; set; }
    }
}
