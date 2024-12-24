using System.ComponentModel.DataAnnotations;
using WALKWAY_ECommerce.Models.Cart_Model;


namespace WALKWAY_ECommerce.Models.Product_Model
{
    public class Product
    {
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Product name is required")]
        public string? ProductName { get; set; }

        [Required(ErrorMessage = "Product  Brand is required")]
        public string? ProductBrand { get; set; }

        [Required(ErrorMessage = "Product  Size is required")]
        public List<int> Sizes { get; set; } = new List<int>();

        [Required(ErrorMessage = "Product description is required")]
        public string? ProductDescription { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be greater than or equal to 0")]
        public decimal ProductPrice { get; set; }


        [Required(ErrorMessage = "Image url is required")]
        [Url(ErrorMessage = "Invalid url Format")]
        public List<string>? ImageUrls { get; set; } 
        [Required]
        public int CategoryId { get; set; }
       
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Stock must be greater than or equal 0")]
        public int Stock { get; set; }


        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be greater than or equal to 0")]
        public decimal MRP { get; set; }
        public virtual Category? Category { get; set; }

        public ICollection<CartItem> CartItems { get; set; }

    }
}
