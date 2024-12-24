using System.ComponentModel.DataAnnotations;

namespace WALKWAY_ECommerce.Models.Cart_Model.CartDto
{
    public class CategoryDto
    {
        public int CategoryId { get; set; }

        [Required]
        public string? CategoryName { get; set; }
    }
}
