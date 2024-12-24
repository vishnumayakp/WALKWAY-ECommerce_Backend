using System.ComponentModel.DataAnnotations;
using WALKWAY_ECommerce.Models.Cart_Model;

namespace WALKWAY_ECommerce.Models.Product_Model.ProductDto
{
    public class GetProductDto
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? ProductBrand { get; set; }
        public string? ProductDescription { get; set; }
        public string Category { get; set; }
        public decimal ProductPrice { get; set; }
        public List<int> Sizes { get; set; }
        public List<string>? ImageUrls { get; set; }
        public int Stock { get; set; }
        public decimal MRP { get; set; }

    }
}
