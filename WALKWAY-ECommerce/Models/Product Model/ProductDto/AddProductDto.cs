using System.ComponentModel.DataAnnotations;

namespace WALKWAY_ECommerce.Models.Product_Model.ProductDto
{
    public class AddProductDto
    {
        [Required]
        public string ProductName { get; set; }
        [Required]
        public string ProductBrand { get; set; }
        [Required]
        public string ProductDescription { get; set; }
        [Required]
        public List<int> Sizes { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be greater than or equal to 0")]
        public decimal ProductPrice { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be greater than or equal to 0")]
        public decimal MRP { get; set; }
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Stock must be greater than or equal to 0")]
        public int Stock { get; set; }
    }
}
