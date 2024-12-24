using System.ComponentModel.DataAnnotations;
using WALKWAY_ECommerce.Models.Order_Model;
using WALKWAY_ECommerce.Models.User_Model;

namespace WALKWAY_ECommerce.Models.Address_Model
{
    public class Address
    {
        public int AddressId { get; set; }

        [Required(ErrorMessage = "FullName Is Required")]
        [StringLength(50,ErrorMessage ="FullName must not Exceed 20 Characters")]
        public string? FullName { get; set; }

        [Required(ErrorMessage = "PhoneNumber Is Required")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "PhoneNumber must be 10 digits")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Pincode Is Required")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "Pincode must be 6 digits")]
        public string? Pincode { get; set; }

        [Required(ErrorMessage = "Housename Is Required")]
        [StringLength(50, ErrorMessage = "HouseName must not Exceed 20 Characters")]
        public string? HouseName { get; set; }


        [Required(ErrorMessage = "Place Is Required")]
        [StringLength(50, ErrorMessage = "Place must not Exceed 20 Characters")]
        public string? Place { get; set; }

        [Required(ErrorMessage = "PostOffice Is Required")]
        [StringLength(50, ErrorMessage = "PostOffice must not Exceed 20 Characters")]
        public string? PostOffice { get; set; }

        [Required(ErrorMessage = "LandMark Is Required")]
        [StringLength(50, ErrorMessage = "LandMark must not Exceed 20 Characters")]
        public string? LandMark { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<OrderMain> Orders { get; set; }
    }
}
